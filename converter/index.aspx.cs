using System;
using System.Collections.Generic;
using System.Web.UI;

namespace converter
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUnits();
                UpdateHistoryDisplay();
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUnits();
            pnlResult.Visible = false;
            pnlExplanation.Visible = false;
            btnExplain.Visible = false;
        }

        private void LoadUnits()
        {
            ddlFrom.Items.Clear();
            ddlTo.Items.Clear();

            string category = ddlCategory.SelectedValue;
            string[] units = new string[] { };

            switch (category)
            {
                case "Length": units = new[] { "Meters", "Kilometers", "Centimeters", "Miles", "Feet", "Inches" }; break;
                case "Weight": units = new[] { "Kilograms", "Grams", "Pounds", "Ounces" }; break;
                case "Temperature": units = new[] { "Celsius", "Fahrenheit", "Kelvin" }; break;
                case "Currency": units = new[] { "USD", "EUR", "GBP", "INR" }; break;
            }

            foreach (var unit in units)
            {
                ddlFrom.Items.Add(unit);
                ddlTo.Items.Add(unit);
            }
        }
        protected void btnBinaryConverter_Click(object sender, EventArgs e)
        {
            // Redirect to Binary Converter page
            Response.Redirect("BinaryConverter.aspx");
        }

        protected void btnUnitConverter_Click(object sender, EventArgs e)
        {
            // Stay on the same page
            Response.Redirect("Index.aspx");
            // You can add code here to reset or show unit converter content if needed
        }

        protected void btnConvert_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtInput.Text, out double inputVal))
            {
                string explanation = "";
                double result = PerformConversion(inputVal, ddlCategory.SelectedValue, ddlFrom.SelectedValue, ddlTo.SelectedValue, out explanation);

                string resultText = $"{inputVal} {ddlFrom.SelectedValue} = {Math.Round(result, 4)} {ddlTo.SelectedValue}";

                lblResult.Text = resultText;
                pnlResult.Visible = true;

                // Store explanation in ViewState to show later if button is clicked
                ViewState["CurrentExplanation"] = explanation;
                btnExplain.Visible = true;
                pnlExplanation.Visible = false; // Hide previous explanation until clicked

                SaveToHistory(resultText);
            }
            else
            {
                lblResult.Text = "Please enter a valid number.";
                pnlResult.Visible = true;
                btnExplain.Visible = false;
            }
        }

        protected void btnExplain_Click(object sender, EventArgs e)
        {
            lblExplanation.Text = ViewState["CurrentExplanation"]?.ToString();
            pnlExplanation.Visible = true;
        }

        private double PerformConversion(double val, string cat, string from, string to, out string explanation)
        {
            explanation = $"Converting {val} {from} to {to}:<br/>";

            if (from == to)
            {
                explanation += "Units are the same. No calculation needed.";
                return val;
            }

            if (cat == "Temperature")
            {
                double celsius;
                if (from == "Celsius") { celsius = val; explanation += "Starting unit is Celsius.<br/>"; }
                else if (from == "Fahrenheit")
                {
                    celsius = (val - 32) * 5 / 9;
                    explanation += $"1. Convert Fahrenheit to Celsius: ({val} - 32) × 5/9 = {Math.Round(celsius, 4)}°C<br/>";
                }
                else
                {
                    celsius = val - 273.15;
                    explanation += $"1. Convert Kelvin to Celsius: {val} - 273.15 = {Math.Round(celsius, 4)}°C<br/>";
                }

                double finalResult = to == "Celsius" ? celsius : (to == "Fahrenheit" ? (celsius * 9 / 5) + 32 : celsius + 273.15);

                if (to == "Fahrenheit") explanation += $"2. Convert Celsius to Fahrenheit: ({Math.Round(celsius, 4)} × 9/5) + 32 = {Math.Round(finalResult, 4)}°F";
                else if (to == "Kelvin") explanation += $"2. Convert Celsius to Kelvin: {Math.Round(celsius, 4)} + 273.15 = {Math.Round(finalResult, 4)}K";
                else explanation += "Target unit is Celsius. Done.";

                return finalResult;
            }

            // Factor-based conversions (Length, Weight, Currency)
            double factorFrom = 1.0, factorTo = 1.0;
            string baseUnit = "";

            Dictionary<string, double> map = new Dictionary<string, double>();

            if (cat == "Length")
            {
                baseUnit = "Meters";
                map = new Dictionary<string, double> { { "Meters", 1 }, { "Kilometers", 1000 }, { "Centimeters", 0.01 }, { "Miles", 1609.34 }, { "Feet", 0.3048 }, { "Inches", 0.0254 } };
            }
            else if (cat == "Weight")
            {
                baseUnit = "Kilograms";
                map = new Dictionary<string, double> { { "Kilograms", 1 }, { "Grams", 0.001 }, { "Pounds", 0.453592 }, { "Ounces", 0.0283495 } };
            }
            else if (cat == "Currency")
            {
                baseUnit = "USD";
                map = new Dictionary<string, double> { { "USD", 1 }, { "EUR", 0.92 }, { "GBP", 0.79 }, { "INR", 83.0 } };
            }

            factorFrom = map[from];
            factorTo = map[to];

            double valInBase = val * factorFrom;
            double finalVal = valInBase / factorTo;

            if (cat == "Currency")
            {
                explanation += $"1. Convert {from} to {baseUnit}: {val} / {factorFrom} = {Math.Round(val / factorFrom, 4)} {baseUnit}<br/>";
                explanation += $"2. Convert {baseUnit} to {to}: {Math.Round(val / factorFrom, 4)} × {factorTo} = {Math.Round(finalVal, 4)} {to}";
                return (val / factorFrom) * factorTo; // Logic fix for currency: Input/FactorFrom * FactorTo
            }
            else
            {
                explanation += $"1. Convert {from} to Base ({baseUnit}): {val} × {factorFrom} = {valInBase} {baseUnit}<br/>";
                explanation += $"2. Convert {baseUnit} to {to}: {valInBase} / {factorTo} = {Math.Round(finalVal, 4)} {to}";
                return finalVal;
            }
        }

        private void SaveToHistory(string entry)
        {
            List<string> history = Session["History"] as List<string> ?? new List<string>();
            history.Insert(0, $"{DateTime.Now.ToString("HH:mm")}: {entry}");
            if (history.Count > 5) history.RemoveAt(5);
            Session["History"] = history;
            UpdateHistoryDisplay();
        }

        private void UpdateHistoryDisplay()
        {
            rptHistory.DataSource = Session["History"] as List<string>;
            rptHistory.DataBind();
        }
    }
}
