using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;

namespace Finance_management
{
    public static class ChartHelper
    {
        public static string BuildDoughnutChart(string canvasId, string title, DataTable data, string labelField, string valueField)
        {
            var labels = new List<string>();
            var values = new List<double>();

            foreach (DataRow row in data.Rows)
            {
                labels.Add(row[labelField].ToString());
                values.Add(System.Convert.ToDouble(row[valueField]));
            }

            var serializer = new JavaScriptSerializer();
            string labelsJson = serializer.Serialize(labels);
            string valuesJson = serializer.Serialize(values);

            var sb = new StringBuilder();
            sb.Append("<script>");
            sb.Append("(function(){");
            sb.Append("var ctx=document.getElementById('").Append(canvasId).Append("');");
            sb.Append("if(!ctx){return;}");
            sb.Append("new Chart(ctx,{type:'doughnut',");
            sb.Append("data:{labels:").Append(labelsJson).Append(",");
            sb.Append("datasets:[{label:'").Append(title).Append("',data:").Append(valuesJson);
            sb.Append(",backgroundColor:['#28a745','#dc3545','#375a7f','#f39c12','#17a2b8','#6f42c1','#fd7e14','#20c997']}]},");
            sb.Append("options:{responsive:true,plugins:{legend:{position:'bottom'}}}});");
            sb.Append("})();");
            sb.Append("</script>");
            return sb.ToString();
        }

        public static string BuildBarChart(string canvasId, string label1, double value1, string label2, double value2)
        {
            var serializer = new JavaScriptSerializer();
            string labelsJson = serializer.Serialize(new[] { label1, label2 });
            string valuesJson = serializer.Serialize(new[] { value1, value2 });

            var sb = new StringBuilder();
            sb.Append("<script>");
            sb.Append("(function(){");
            sb.Append("var ctx=document.getElementById('").Append(canvasId).Append("');");
            sb.Append("if(!ctx){return;}");
            sb.Append("new Chart(ctx,{type:'bar',");
            sb.Append("data:{labels:").Append(labelsJson).Append(",");
            sb.Append("datasets:[{label:'Amount',data:").Append(valuesJson);
            sb.Append(",backgroundColor:['#28a745','#dc3545']}]},");
            sb.Append("options:{responsive:true,plugins:{legend:{display:false}},scales:{y:{beginAtZero:true}}}});");
            sb.Append("})();");
            sb.Append("</script>");
            return sb.ToString();
        }
    }
}
