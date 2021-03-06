using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Nexusat.Utils.CalendarGenerator
{
    /// <summary>
    ///     Represents a dump of a calendar period
    /// </summary>
    [XmlRoot(ElementName = "CalendarDays")]
    public sealed class CalendarDays : List<CalendarDays.Day>
    {
        /// <summary>
        ///     Returns an XML representation of the calendar days table
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public string ToXml(XmlWriterSettings settings = null)
        {
            /* XML Layout
            <CalendarDays>
	            <Day date="2020-01-01" isWorkingDay="true|false">
		            <Description></Description>  <!-- missing if null -->
		            <WorkingPeriods>
			            <TimePeriod begin="08:00" end="12:00" />
			            <TimePeriod begin="14:00" end="18:00" />
		            </WorkingPeriods>
	            </Day>
            <CalendarDays>
            */
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var ser = new XmlSerializer(typeof(CalendarDays));
            var sb = new StringBuilder();
            using var writer = XmlWriter.Create(sb, settings);
            ser.Serialize(writer, this, ns);
            writer.Flush();
            return sb.ToString();
        }

        /// <summary>
        ///     Returns a JSON representation of the calendar days table
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        public string ToJson(bool indent = false)
        {
            /* JSON Layout
            [{
                "date": "ISO_DATE",
                "isWorkingDay": true | false,
                "description": "",
                "workingPeriods": [{
                    "begin": "08:00",
                    "end": "12:00"
                },{
                    "begin": "14:00",
                    "end": "18:00"
                },
                ]
            }]
            */
            var opts = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = indent
            };
            var json = JsonSerializer.Serialize(this, opts);
            return json;
        }

        public class TimePeriod
        {
            [XmlAttribute(AttributeName = "begin")]
            public string Begin { get; set; }

            [XmlAttribute(AttributeName = "end")] public string End { get; set; }
        }

        public class Day
        {
            [XmlAttribute(AttributeName = "date")] public string Date { get; set; }

            [XmlElement] public string Description { get; set; }

            [XmlAttribute(AttributeName = "isWorkingDay")]
            public bool IsWorkingDay { get; set; }

            public List<TimePeriod> WorkingPeriods { get; set; }
        }
    }
}