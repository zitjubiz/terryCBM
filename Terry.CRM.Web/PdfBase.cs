using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using PDFHelper.Helper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Terry.CRM.Entity;

namespace Terry.CRM.Web
{
    /// <summary>
    /// Chunk : 块，PDF文档中描述的最小原子元素
    /// Phrase : 短语，Chunk的集合
    /// Paragraph : 段落，一个有序的Phrase集合
    /// </summary>
    public abstract class PdfBase : TextSharpHelper
    {
        public PdfBase(string fileName) : base(fileName) { }

        /// <summary>
        /// 四号
        /// </summary>
        protected virtual Font TextFont
        {
            get
            {
                return CreateDefaultFont(四号);
            }
        }

        /// <summary>
        /// 四号UNDERLINE
        /// </summary>
        protected virtual Font TextAndUnderLineFont
        {
            get
            {
                return CreateDefaultFont(四号, Font.NORMAL | Font.UNDERLINE);
            }
        }
        /// <summary>
        /// 三号BOLD
        /// </summary>
        protected virtual Font ProjectFont
        {
            get
            {
                return CreateDefaultFont(三号, Font.NORMAL | Font.BOLD);
            }
        }
        /// <summary>
        /// 四号BOLD
        /// </summary>
        protected virtual Font TitleFont
        {
            get
            {
                return CreateDefaultFont(四号, Font.NORMAL | Font.BOLD);
            }
        }
        /// <summary>
        /// 小三号BOLD
        /// </summary>
        protected virtual Font ProjectNameDetail
        {
            get
            {
                return CreateDefaultFont(小三号, Font.NORMAL | Font.BOLD);
            }
        }
        /// <summary>
        /// 小三号 BOLD UNDERLINE
        /// </summary>
        protected virtual Font ProjectNameFont
        {
            get
            {
                return CreateDefaultFont(小三号, Font.BOLD | Font.UNDERLINE);
            }
        }



        private Dictionary<string, object> _keyValues;

        public void Fill(Dictionary<string, object> dictionary)
        {
            _keyValues = dictionary;
            FillPdfFile(FillDocument);
        }

        void FillDocument(Document doc)
        {
            FillDocument(doc, _keyValues);
        }

        protected abstract void FillDocument(Document doc, Dictionary<string, object> dictionary);


        protected Paragraph CreateParagraph(int alignment)
        {
            return CreateParagraphAndDestination(alignment, null, null);
        }

        protected Paragraph CreateParagraph()
        {
            return CreateParagraphAndDestination(PdfContentByte.ALIGN_LEFT, null, null);
        }

        protected Paragraph CreateParagraphAndDestination(int alignment, string name, Font font)
        {
            PdfOutline outline;
            return CreateParagraphAndDestination(alignment, name, font, out outline);
        }

        protected Paragraph CreateParagraphAndDestination(string name, Font font, PdfOutline rootline)
        {
            PdfOutline outline;
            return CreateParagraphAndDestination(PdfContentByte.ALIGN_LEFT, name, font, rootline, out outline);
        }

        protected Paragraph CreateParagraphAndDestination(int alignment, string name, Font font, PdfOutline rootline)
        {
            PdfOutline outline;
            return CreateParagraphAndDestination(alignment, name, font, rootline, out outline);
        }

        protected Paragraph CreateParagraphAndDestination(string name, Font font, PdfOutline rootline, out PdfOutline outline)
        {
            return CreateParagraphAndDestination(PdfContentByte.ALIGN_LEFT, name, font, rootline, out outline);
        }
        protected Paragraph CreateParagraphAndDestination(int alignment, string name, Font font, PdfOutline rootline, out PdfOutline outline)
        {
            switch (alignment)
            {
                case PdfContentByte.ALIGN_LEFT:
                case PdfContentByte.ALIGN_CENTER:
                case PdfContentByte.ALIGN_RIGHT:
                    break;
                default:
                    alignment = PdfContentByte.ALIGN_LEFT;
                    break;
            }

            Paragraph ph = CreateElementAndDestination<Paragraph>(name, font, rootline, out outline);
            ph.Alignment = alignment;
            ph.SetLeading(2, 2);
            return ph;
        }

        protected Paragraph CreateParagraphAndDestination(int alignment, string name, Font font, out PdfOutline outline)
        {
            return CreateParagraphAndDestination(alignment, name, font, null, out outline);
        }

        protected Phrase CreatePhraseAndDestination(string name, Font font, PdfOutline rootline)
        {
            PdfOutline outline;
            return CreateElementAndDestination<Phrase>(name, font, rootline, out outline);
        }

        protected TElement CreateElementAndDestination<TElement>(string name, Font font, PdfOutline inline, out PdfOutline outline) where TElement : class, IElement, new()
        {
            Chunk chk = null;
            outline = null;
            if (!string.IsNullOrEmpty(name))
            {
                chk = new Chunk(name, font);
                outline = inline == null ? SetDestination(chk) : SetDestination(inline, chk);
            }

            if (typeof(TElement) == typeof(Paragraph))
            {

                Paragraph ph = chk == null ? new Paragraph() : new Paragraph(chk);
                return ph as TElement;
            }
            if (typeof(TElement) == typeof(Phrase))
            {
                return (chk == null ? new Phrase() : new Phrase(chk)) as TElement;
            }

            return null;
        }

        protected TElement CreateElementAndDestination<TElement>(string name, Font font, out PdfOutline outline) where TElement : class, IElement, new()
        {
            return CreateElementAndDestination<TElement>(name, font, null, out outline);
        }

        protected Paragraph CreateParagraphAndDestination(string name, Font font)
        {
            return CreateParagraphAndDestination(PdfContentByte.ALIGN_LEFT, name, font);
        }

        protected void AddNewLine(Paragraph ph, int count)
        {
            for (int i = 0; i < count; i++)
            {
                ph.Add(Environment.NewLine);
            }
        }

        protected void AddNewLine(Paragraph ph)
        {
            ph.Add(Environment.NewLine);
        }
    }

    public class TicketInvoicePdf : PdfBase
    {
        public TicketInvoicePdf(string fileName)
            : base(fileName) { }

        protected override void FillDocument(Document doc, Dictionary<string, object> dictionary)
        {
            PdfOutline rootline;
            Paragraph ph = CreateParagraphAndDestination(PdfContentByte.ALIGN_CENTER, "Fujian Int. Travel Tang (FITT)", TextFont, out rootline);
            ph.Add(new Phrase(dictionary["部门地址"].ToString(), TextFont));
            AddNewLine(ph, 1);

            Image jpeg = Image.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "Invoice\\logo.png");
            jpeg.Alignment = Element.ALIGN_CENTER;
            doc.Add(jpeg);

            ph.Add(new Phrase(dictionary["部门名称"].ToString(), TextFont));
            ph.Add(new Phrase(dictionary["预订日期"].ToString(), TextFont));
            AddNewLine(ph, 2);

            ph.Add(new Phrase(dictionary["客户全名"].ToString(), TextFont));
            ph.Add(new Phrase(dictionary["客户地址"].ToString(), TextFont));
            ph.Add(new Phrase(dictionary["电话"].ToString(), TextFont));
            ph.Add(new Phrase(dictionary["电邮"].ToString(), TextFont));

            AddNewLine(ph, 2);

            doc.Add(ph);

            ph = CreateParagraphAndDestination("Rechnung Nr." + dictionary["内部订单号"].ToString(), TitleFont, rootline);
            doc.Add(ph);

            ph = CreateParagraph();
            ph.FirstLineIndent = TextFont.Size * 2;
            AddNewLine(ph, 1);
            ph.Add(new Phrase("(bitte bei der Zahlung Rechnungsnummer angeben)", TextFont));
            ph.Add(new Phrase("Alle Angaben in EUR", TextFont));

            AddNewLine(ph, 1);
            doc.Add(ph);

            PdfOutline second;
            ph = CreateParagraphAndDestination("Airlines" + dictionary["航空公司"].ToString(), TitleFont, rootline, out second);

            AddNewLine(ph, 1);
            doc.Add(ph);

            ph = CreateParagraph();
            ph.IndentationLeft = TextFont.Size * 2;

            IList<BillTicketPerson> PersonList = dictionary["乘客名单"] as IList<BillTicketPerson>;
            for (int i = 0; i < PersonList.Count; i++)
            {
                if (PersonList[i].IsShowOnInvoice)
                {
                    ph.Add(new Phrase(PersonList[i].OwnerName, TextFont));
                    AddNewLine(ph, 1);
                }
            }

            IList<BillTicketTour> TourList = dictionary["行程信息"] as IList<BillTicketTour>;
            for (int i = 0; i < TourList.Count; i++)
            {

                ph.Add(new Phrase(TourList[i].FlightNum, TextFont));
                ph.Add(new Phrase(TourList[i].FlightDate, TextFont));
                ph.Add(new Phrase(TourList[i].FlightFrom, TextFont));
                ph.Add(new Phrase(TourList[i].FlightTo, TextFont));
                ph.Add(new Phrase(TourList[i].FlightStartTime, TextFont));
                ph.Add(new Phrase(TourList[i].FlightEndTime, TextFont));
                AddNewLine(ph, 1);

            }

            doc.Add(ph);




        }
    }
}