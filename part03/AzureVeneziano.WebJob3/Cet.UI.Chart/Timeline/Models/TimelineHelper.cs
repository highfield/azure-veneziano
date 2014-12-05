//#define LOGFILE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cet.UI.Chart
{
    public static partial class TimelineHelper
    {
        public static readonly Guid TickClassId = Guid.Parse("{084F6EDD-D4E7-45E9-A958-7BF91E1F8DC4}");

#if LOGFILE
        private static System.IO.StreamWriter _file;
#endif


        public static ChartTickSegmentCollection CalculateTicks(TimelineCalculateTickArgs args)
        {
            //prepara una lista di segmenti
            var segmentList = new List<TimelineCalculateSegmentTickArgs>();

            if (args.SegmentInfo.IsValid)
            {
                //prepara l'enumeratore per i gaps
                var egaps = args.Gaps.GetEnumerator();

                //suddivide l'intero intervallo temporale in tanti
                //segmenti, ciascuno diviso da un gap
                long lowerProgression = args.SegmentInfo.LowerBound;
                long upperProgression = 0;
                long lowerUserTimestamp = args.SegmentInfo.LowerBound;
                long upperUserTimestamp = 0;
                TimelineGapInfo gap = null;

                do
                {
                    long segmentExtent = 0;
                    long gapDuration = 0;

                    if (egaps.MoveNext())
                    {
                        //considera il nuovo gap
                        gap = egaps.Current;
                        segmentExtent = gap.StartProgression - lowerProgression;
                        gapDuration = gap.Duration;
                    }
                    else
                    {
                        //terminati i gap
                        gap = null;
                        segmentExtent = args.SegmentInfo.UpperBound - lowerProgression;
                    }

                    upperProgression = lowerProgression + segmentExtent;
                    upperUserTimestamp = lowerUserTimestamp + segmentExtent;

                    if (lowerProgression <= args.SegmentInfo.UpperBound &&
                        upperProgression >= args.SegmentInfo.LowerBound)
                    {
                        //taglia il segmento secondo la finestra effettiva
                        var lo = Math.Max(
                            lowerProgression,
                            args.SegmentInfo.LowerBound);

                        var hi = Math.Min(
                            upperProgression,
                            args.SegmentInfo.UpperBound);

                        var lowerBound = lowerUserTimestamp + lo - lowerProgression;
                        var upperBound = upperUserTimestamp + hi - upperProgression;

                        //crea ed aggiunge il segmento alla lista
                        var segment = new TimelineCalculateSegmentTickArgs();
                        segment.LowerBound = lowerBound;
                        segment.UpperBound = upperBound;
                        segmentList.Add(segment);

                        if (gap != null &&
                            upperProgression < args.SegmentInfo.UpperBound)
                        {
                            segment.GapDuration = gapDuration;
                        }
                    }

                    lowerUserTimestamp = upperUserTimestamp + gapDuration;
                    lowerProgression = upperProgression;
                } while (
                    gap != null && 
                    upperProgression < args.SegmentInfo.UpperBound
                    );
            }

            //calcola le suddivisioni segmento per segmento
            var result = new ChartTickSegmentCollection(TickClassId);
            //var segmentResultList = new List<TimelineCalculateSegmentTickResult>();
            var pixelToProgressionRatio = args.SegmentInfo.Width / (double)args.SegmentInfo.Extent;
            double startPixel = args.SegmentInfo.StartPixel;

            foreach (var segment in segmentList)
            {
                //stabilisce l'ampiezza del segmento grafico
                var width = (int)((segment.UpperBound - segment.LowerBound) * pixelToProgressionRatio);
                segment.StartPixel = startPixel;
                segment.EndPixel = startPixel + width - 1;
                startPixel += width;
                //Console.WriteLine(
                //    "lb={0}; ub={1}; sp={2}; ep={3}",
                //    new DateTime(segment.LowerBound),
                //    new DateTime(segment.UpperBound),
                //    segment.StartPixel,
                //    segment.EndPixel);

                var segmentResult = TimelineHelper.CalculateSegmentTicks(
                    args,
                    segment);

                result.Add(segmentResult);
            }

            //crea un'istanza per il risultato
            //result.InputArgs = args;
            //result.Segments = segmentResultList;
            return result;
        }


        /// <summary>
        /// Dato un intervallo temporale ed un'area bitmap, restituisce
        /// un'enumerazione di istanze <see cref="TimelineTickInfo"/> che descrivono
        /// le varie demarcazioni lungo le ascisse
        /// </summary>
        /// <param name="args"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        private static ChartTickSegment CalculateSegmentTicks(
            TimelineCalculateTickArgs args,
            TimelineCalculateSegmentTickArgs segment)
        {
            //controllo preliminare di validità
            if (segment != null &&
                segment.IsValid)
            {
                //ricava lo snapper di riferimento 
                //(dal quale basare tutti gli altri)
                var referenceSnapper = TimelineHelper.GetReferenceSnapper(
                    args,
                    segment);

                if (referenceSnapper != null)
                {
                    //accumula tutte le info possibili per il segmento corrente
                    var collection = TimelineHelper.AccumulateTickInfo(
                        args,
                        segment,
                        referenceSnapper);

#if LOGFILE
                    _file.WriteLine("----------------------------------------------");
                    _file.Close();
                    _file = null;
#endif

#if false
            //mostra risultati
            Console.WriteLine("width={0}", width);
            foreach (var info in result)
            {
                Console.WriteLine(
                    "Info: X={0}; Idx={1}; Dt={2}; Unit={3}",
                    info.X,
                    info.TopIndex,
                    new DateTime(info.Ticks),
                    _steppers[info.TopIndex].Unit);
            }
#endif

                    //calcola l'importanza e la assegna a ciascun istanza info
                    var sortedUnitList = (from info in collection
                                          where info.PixelPosition > 0
                                          let unit = _snappers[info.TopIndex].Unit
                                          orderby (int)unit
                                          select unit)
                                          .Distinct()
                                          .ToList();

                    for (int importance = 0; importance < sortedUnitList.Count; importance++)
                    {
                        var unit = sortedUnitList[importance];

                        foreach (var info in collection)
                        {
                            if (_snappers[info.TopIndex].Unit == unit)
                                info.Priority = importance;
                        }
                    }

                    //se possibile procede alla formattazione
                    if (args.Formatter != null)
                        args.Formatter(collection);

                    //finalmente restituisce i dati
                    collection.IsValid = true;
                    collection.PriorityCount = sortedUnitList.Count;
                    return collection;
                }
            }

            //restituisce un'istanza priva di contenuto
            return new ChartTickSegment();
        }


        /// <summary>
        /// Cerca lo snapper minimo da usare come riferimento
        /// </summary>
        /// <param name="args"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        private static TimelineSnapper GetReferenceSnapper(
            TimelineCalculateTickArgs args,
            TimelineCalculateSegmentTickArgs segment)
        {
            //calcola il numero massimo di divisioni
            double maxDivs = segment.Width / (double)args.MinimumDivisionLength;

            //ricava l'ampiezza minima di una divisione
            var divExtent = (long)(segment.Extent / maxDivs);

            //cerca lo stepper minimo dal quale partire
            for (int ix = 0; ix < _snappers.Count; ix++)
            {
                if (_snappers[ix].NominalExtent > divExtent)
                    return _snappers[ix];
            }

            return null;
        }


        /// <summary>
        /// Accumula tutte le istanze <see cref="TimelineTickInfo"/> possibili entro l'intervallo specificato
        /// </summary>
        /// <param name="args"></param>
        /// <param name="segment"></param>
        /// <param name="referenceSnapper"></param>
        /// <returns></returns>
        private static ChartTickSegment AccumulateTickInfo(
            TimelineCalculateTickArgs args,
            TimelineCalculateSegmentTickArgs segment,
            TimelineSnapper referenceSnapper)
        {
            var collection = new ChartTickSegment();

            //calcola l'estensione temporale di un pixel logico
            var pixelExtent = (long)(segment.Extent / (segment.Width - 1));

#if LOGFILE
            _file = System.IO.File.CreateText(@"C:\Users\Mario\Documents\test.txt");
            _file.WriteLine("lower={0}", lowerBound);
            _file.WriteLine("upper={0}", upperBound);
            _file.WriteLine("width={0}", pixelEnd - pixelStart + 1);
            _file.WriteLine("pixelExtent={0}; {1}", pixelExtent, TimeSpan.FromTicks(pixelExtent));
            _file.WriteLine("pixelExtent/2={0}; {1}", pixelExtent / 2, TimeSpan.FromTicks(pixelExtent / 2));
#endif

            var endX = (int)(segment.Width - 1);

            for (int x = 0; x <= endX; x++)
            {
                //calcola l'istante nominale ed i due estremi
                long tnom = segment.LowerBound + pixelExtent * x;

                //calcola i due estremi attorno al valore nominale
                long tmin = tnom - pixelExtent / 2;
                long tmax = tmin + pixelExtent;

#if LOGFILE
                _file.WriteLine(
                    "X={0}; t={1}; {2}; min={3}; max={4}",
                    x,
                    t,
                    new DateTime(t),
                    new DateTime(tmin),
                    new DateTime(tmax));
#endif

                TimelineTickInfo info = null;

                //se per il segmento corrente è previsto un gap,
                //allora assicura la presenza dell'istanza info
                if (x == endX &&
                    segment.GapDuration.HasValue)
                {
                    info = new TimelineTickInfo();
                    info.PixelPosition = x + segment.StartPixel;
                    info.Gap = segment.GapDuration;
                }

                //prova tutti gli snappers da quello di riferimento in poi
                for (int y = _snappers.IndexOf(referenceSnapper); y < _snappers.Count; y++)
                {
                    //gli snappers successivi a quello di riferimento
                    //potrebbero non essere disponibili, quindi
                    //controlla l'effettiva usabilità
                    var testSnapper = _snappers[y];

                    if (object.Equals(referenceSnapper, testSnapper) == false &&
                        testSnapper.CanBeUsed(referenceSnapper) == false)
                    {
                        continue;
                    }

                    //richiede la versione arrotondata del valore nominale
                    var snapped = testSnapper.Snap(tnom);

                    //controlla se cade entro l'intervallo
                    if (tmin <= snapped &&
                        snapped <= tmax)
                    {
                        //aggiorna l'istanza info con i migliori parametri trovati
                        if (info == null)
                        {
                            info = new TimelineTickInfo();
                            info.PixelPosition = x + segment.StartPixel;
                        }

                        info.TopIndex = y;
                        info.UserTimestamp = new DateTime(snapped);
                    }
#if LOGFILE
                    _file.WriteLine(
                        "Y={0}; Ext={1}; {2}; Snap={3}; {4}; Info={5}",
                        y,
                        _snappers[y].NominalExtent,
                        TimeSpan.FromTicks(_snappers[y].NominalExtent),
                        snapped,
                        new DateTime(snapped),
                        info);
#endif
                }

                //restituisce l'istanza se è non nulla
                if (info != null)
                    collection.Add(info);
            }

            return collection;
        }


        /// <summary>
        /// Formatta la data in modo standard "giorno-mese"
        /// (preservando l'ordine e la simbologia della cultura corrente),
        /// ma usando la versione abbreviata del mese
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static string ShortMonthDayPattern(DateTime dt)
        {
            var result = dt.ToString("M");
            var monthName = dt.ToString("MMMM");
            return result.Replace(
                monthName,
                dt.ToString("MMM"));
        }


        public static DateTime CalculateActualTimestamp(
            TimelineCalculateTickArgs args,
            long progression)
        {
            //TODO: tenere conto dei gaps
            return new DateTime(progression);
        }

    }
}
