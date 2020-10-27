using RealEstate.Entities.ViewModels;
using System;

using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Web.HtmlHelpers
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl, string dropdowncssclass = "", string labelcssclass = "pager-label")
        {
            StringBuilder result = new StringBuilder();
            int prevPageNo, nextPageNo, lastPageNo;
            prevPageNo = pagingInfo.CurrentPage > 1 ? (pagingInfo.CurrentPage - 1) : 1;
            nextPageNo = pagingInfo.CurrentPage < pagingInfo.TotalPages ? (pagingInfo.CurrentPage + 1) : pagingInfo.CurrentPage;
            lastPageNo = pagingInfo.TotalPages;

            result.Append("&nbsp;<label class= page-size-label '" + labelcssclass + "'>Page Size:</label>");
            result.Append("&nbsp;&nbsp;<select class='page-size " + dropdowncssclass + "'>");

            if (pagingInfo.ItemsPerPage == 10)
            {
                result.Append("<option value='10' selected='selected'>10</option>");
                result.Append("<option value='20'>20</option>");
                result.Append("<option value='30'>30</option>");
                result.Append("<option value='50'>50</option>");
            }
            else if (pagingInfo.ItemsPerPage == 20)
            {
                result.Append("<option value='10'>10</option>");
                result.Append("<option value='20' selected='selected'>20</option>");
                result.Append("<option value='30'>30</option>");
                result.Append("<option value='50'>50</option>");
            }
            else if (pagingInfo.ItemsPerPage == 30)
            {
                result.Append("<option value='10'>10</option>");
                result.Append("<option value='20'>20</option>");
                result.Append("<option value='30' selected='selected'>30</option>");
                result.Append("<option value='50'>50</option>");
            }
            else if (pagingInfo.ItemsPerPage == 50)
            {
                result.Append("<option value='10'>10</option>");
                result.Append("<option value='20'>20</option>");
                result.Append("<option value='30'>30</option>");
                result.Append("<option value='50' selected='selected'>50</option>");
            }
            result.Append("</select>&nbsp;&nbsp;&nbsp;");
            TagBuilder tagFirst = new TagBuilder("a");
            if (pagingInfo.CurrentPage == 1)
            {
                tagFirst.SetInnerText("");
                tagFirst.AddCssClass("ns-page-link-disabled");
            }
            else
            {
                tagFirst.MergeAttribute("href", pageUrl(1));
                tagFirst.SetInnerText("");
                tagFirst.AddCssClass("ns-page-link");
            }
            tagFirst.AddCssClass("glyphicon glyphicon-step-backward");
            result.Append(tagFirst.ToString());

            TagBuilder tagPrev = new TagBuilder("a");
            if (pagingInfo.CurrentPage == 1)
            {
                tagPrev.SetInnerText("");
                tagPrev.AddCssClass("ns-page-link-disabled");
            }
            else
            {
                tagPrev.MergeAttribute("href", pageUrl(prevPageNo));
                tagPrev.SetInnerText("");
                tagPrev.AddCssClass("ns-page-link");
            }
            tagPrev.AddCssClass("glyphicon glyphicon-chevron-left");
            result.Append(tagPrev.ToString());

            result.Append("&nbsp;<label class='" + labelcssclass + "'>Page</label>");
            result.Append("&nbsp;&nbsp;<select class='page-number " + dropdowncssclass + "'>");
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                if (i == pagingInfo.CurrentPage)
                {
                    result.Append("<option value='" + i + "' selected='selected'>" + i + "</option>");
                }
                else
                {
                    result.Append("<option value='" + i + "' >" + i + "</option>");
                }
            }
            result.Append("</select>");
            result.Append("&nbsp;&nbsp;<label class='" + labelcssclass + "'>of</label>");
            result.Append("&nbsp;&nbsp;<label class='" + labelcssclass + "'>" + pagingInfo.TotalPages + "</label>&nbsp;&nbsp;");

            TagBuilder tagNext = new TagBuilder("a");
            if (pagingInfo.CurrentPage == pagingInfo.TotalPages)
            {
                tagNext.SetInnerText("");
                tagNext.AddCssClass("ns-page-link-disabled");
            }
            else
            {
                tagNext.MergeAttribute("href", pageUrl(nextPageNo));
                tagNext.SetInnerText("");
                tagNext.AddCssClass("ns-page-link");
            }
            tagNext.AddCssClass("glyphicon glyphicon-chevron-right");
            result.Append(tagNext.ToString());

            TagBuilder tagLast = new TagBuilder("a");
            if (pagingInfo.CurrentPage == lastPageNo)
            {
                tagLast.SetInnerText("");
                tagLast.AddCssClass("ns-page-link-disabled");
            }
            else
            {
                tagLast.MergeAttribute("href", pageUrl(lastPageNo));
                tagLast.SetInnerText("");
                tagLast.AddCssClass("ns-page-link");
            }
            tagLast.AddCssClass("glyphicon glyphicon-step-forward");
            result.Append(tagLast.ToString());
            return MvcHtmlString.Create(result.ToString());
        }
        public static MvcHtmlString ModifiedPageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl, string dropdowncssclass = "", string labelcssclass = "pager-label")
        {
            StringBuilder result = new StringBuilder();
            int prevPageNo, nextPageNo, lastPageNo;
            prevPageNo = pagingInfo.CurrentPage > 1 ? (pagingInfo.CurrentPage - 1) : 1;
            nextPageNo = pagingInfo.CurrentPage < pagingInfo.TotalPages ? (pagingInfo.CurrentPage + 1) : pagingInfo.CurrentPage;
            lastPageNo = pagingInfo.TotalPages;

            result.Append("&nbsp;<label class= page-size-label '" + labelcssclass + "'>Page Size:</label>");
            result.Append("&nbsp;&nbsp;<select class='pg-sz " + dropdowncssclass + "'>");

            if (pagingInfo.ItemsPerPage == 10)
            {
                result.Append("<option value='10' selected='selected'>10</option>");
                result.Append("<option value='20'>20</option>");
                result.Append("<option value='30'>30</option>");
                result.Append("<option value='50'>50</option>");
            }
            else if (pagingInfo.ItemsPerPage == 20)
            {
                result.Append("<option value='10'>10</option>");
                result.Append("<option value='20' selected='selected'>20</option>");
                result.Append("<option value='30'>30</option>");
                result.Append("<option value='50'>50</option>");
            }
            else if (pagingInfo.ItemsPerPage == 30)
            {
                result.Append("<option value='10'>10</option>");
                result.Append("<option value='20'>20</option>");
                result.Append("<option value='30' selected='selected'>30</option>");
                result.Append("<option value='50'>50</option>");
            }
            else if (pagingInfo.ItemsPerPage == 50)
            {
                result.Append("<option value='10'>10</option>");
                result.Append("<option value='20'>20</option>");
                result.Append("<option value='30'>30</option>");
                result.Append("<option value='50' selected='selected'>50</option>");
            }
            result.Append("</select>&nbsp;&nbsp;&nbsp;");
            TagBuilder tagFirst = new TagBuilder("a");
            if (pagingInfo.CurrentPage == 1)
            {
                tagFirst.SetInnerText("");
                tagFirst.AddCssClass("ns-page-link-disabled");
            }
            else
            {
                tagFirst.MergeAttribute("href", pageUrl(1));
                tagFirst.SetInnerText("");
                tagFirst.AddCssClass("ns-page-link");
            }
            tagFirst.AddCssClass("glyphicon glyphicon-step-backward");
            result.Append(tagFirst.ToString());

            TagBuilder tagPrev = new TagBuilder("a");
            if (pagingInfo.CurrentPage == 1)
            {
                tagPrev.SetInnerText("");
                tagPrev.AddCssClass("ns-page-link-disabled");
            }
            else
            {
                tagPrev.MergeAttribute("href", pageUrl(prevPageNo));
                tagPrev.SetInnerText("");
                tagPrev.AddCssClass("ns-page-link");
            }
            tagPrev.AddCssClass("glyphicon glyphicon-chevron-left");
            result.Append(tagPrev.ToString());

            result.Append("&nbsp;<label class='" + labelcssclass + "'>Page</label>");
            result.Append("&nbsp;&nbsp;<select class='pg-no " + dropdowncssclass + "'>");
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                if (i == pagingInfo.CurrentPage)
                {
                    result.Append("<option value='" + i + "' selected='selected'>" + i + "</option>");
                }
                else
                {
                    result.Append("<option value='" + i + "' >" + i + "</option>");
                }
            }
            result.Append("</select>");
            result.Append("&nbsp;&nbsp;<label class='" + labelcssclass + "'>of</label>");
            result.Append("&nbsp;&nbsp;<label class='" + labelcssclass + "'>" + pagingInfo.TotalPages + "</label>&nbsp;&nbsp;");

            TagBuilder tagNext = new TagBuilder("a");
            if (pagingInfo.CurrentPage == pagingInfo.TotalPages)
            {
                tagNext.SetInnerText("");
                tagNext.AddCssClass("ns-page-link-disabled");
            }
            else
            {
                tagNext.MergeAttribute("href", pageUrl(nextPageNo));
                tagNext.SetInnerText("");
                tagNext.AddCssClass("ns-page-link");
            }
            tagNext.AddCssClass("glyphicon glyphicon-chevron-right");
            result.Append(tagNext.ToString());

            TagBuilder tagLast = new TagBuilder("a");
            if (pagingInfo.CurrentPage == lastPageNo)
            {
                tagLast.SetInnerText("");
                tagLast.AddCssClass("ns-page-link-disabled");
            }
            else
            {
                tagLast.MergeAttribute("href", pageUrl(lastPageNo));
                tagLast.SetInnerText("");
                tagLast.AddCssClass("ns-page-link");
            }
            tagLast.AddCssClass("glyphicon glyphicon-step-forward");
            result.Append(tagLast.ToString());
            return MvcHtmlString.Create(result.ToString());
        }
    }
}