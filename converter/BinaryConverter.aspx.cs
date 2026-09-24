using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace converter
{
    public partial class BinaryConverter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateValidationHint();
                UpdateHistoryDisplay();
            }
        }

        protected void ddlConversionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateValidationHint();
            pnlResults.Visible = false;
            pnlError.Visible = false;
            pnlExplanation.Visible = false;
            txtInput.Text = "";
        }

        private void UpdateValidationHint()
        {
            string type = ddlConversionType.SelectedValue;
            string hint = "";
            string label = "Enter Value";

            switch (type)
            {
                case "BinaryToDecimal":
                case "BinaryToOctal":
                case "BinaryToHex":
                    hint = "Enter a valid binary number (e.g., 1010, 11001)";
                    label = "Enter Binary Value";
                    break;
                case "DecimalToBinary":
                case "DecimalToOctal":
                case "DecimalToHex":
                    hint = "Enter a valid decimal number (e.g., 42, 255)";
                    label = "Enter Decimal Value";
                    break;
                case "OctalToBinary":
                case "OctalToDecimal":
                case "OctalToHex":
                    hint = "Enter a valid octal number (digits 0-7, e.g., 177, 52)";
                    label = "Enter Octal Value";
                    break;
                case "HexToBinary":
                case "HexToDecimal":
                case "HexToOctal":
                    hint = "Enter a valid hexadecimal number (0-9, A-F, e.g., 1A, FF)";
                    label = "Enter Hexadecimal Value";
                    break;
            }

            lblValidationHint.InnerText = hint;
            lblInputLabel.InnerText = label;
        }

        protected void btnConvert_Click(object sender, EventArgs e)
        {
            pnlError.Visible = false;
            pnlResults.Visible = false;
            pnlExplanation.Visible = false;

            string input = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                ShowError("Please enter a value to convert.");
                return;
            }

            string conversionType = ddlConversionType.SelectedValue;
            string explanation = "";
            ConversionResult result = null;

            try
            {
                result = PerformConversion(input, conversionType, out explanation);

                // Display results
                divBinaryResult.InnerText = result.Binary;
                divDecimalResult.InnerText = result.Decimal;
                divOctalResult.InnerText = result.Octal;
                divHexResult.InnerText = result.Hexadecimal;

                // Display bit visualization
                DisplayBits(result.Binary);

                // Show explanation
                lblExplanation.Text = explanation;
                pnlExplanation.Visible = true;

                pnlResults.Visible = true;

                // Save to history
                SaveToHistory($"{input} → {result.GetResultString(conversionType)}");
            }
            catch (Exception ex)
            {
                ShowError($"Invalid input: {ex.Message}");
            }
        }

        private ConversionResult PerformConversion(string input, string conversionType, out string explanation)
        {
            explanation = "";
            long decimalValue = 0;
            string binary = "", octal = "", hex = "";

            switch (conversionType)
            {
                case "BinaryToDecimal":
                    decimalValue = Convert.ToInt64(input, 2);
                    binary = input;
                    octal = Convert.ToString(decimalValue, 8);
                    hex = Convert.ToString(decimalValue, 16).ToUpper();
                    explanation = $"Binary {input} = Decimal {decimalValue}\n\n" +
                                 $"Conversion steps:\n" +
                                 $"1. Each binary digit represents a power of 2\n" +
                                 $"2. {input} = {GetBinaryToDecimalExplanation(input)}\n" +
                                 $"3. Sum = {decimalValue}";
                    break;

                case "BinaryToOctal":
                    decimalValue = Convert.ToInt64(input, 2);
                    binary = input;
                    octal = Convert.ToString(decimalValue, 8);
                    hex = Convert.ToString(decimalValue, 16).ToUpper();
                    explanation = $"Binary {input} = Octal {octal}\n\n" +
                                 $"Conversion steps:\n" +
                                 $"1. Convert binary to decimal: {input} = {decimalValue}\n" +
                                 $"2. Group binary digits into sets of 3 from right\n" +
                                 $"3. Each group converts to octal digit";
                    break;

                case "BinaryToHex":
                    decimalValue = Convert.ToInt64(input, 2);
                    binary = input;
                    octal = Convert.ToString(decimalValue, 8);
                    hex = Convert.ToString(decimalValue, 16).ToUpper();
                    explanation = $"Binary {input} = Hex {hex}\n\n" +
                                 $"Conversion steps:\n" +
                                 $"1. Convert binary to decimal: {input} = {decimalValue}\n" +
                                 $"2. Group binary digits into sets of 4 from right\n" +
                                 $"3. Each group converts to hex digit";
                    break;

                case "DecimalToBinary":
                    decimalValue = long.Parse(input);
                    binary = Convert.ToString(decimalValue, 2);
                    octal = Convert.ToString(decimalValue, 8);
                    hex = Convert.ToString(decimalValue, 16).ToUpper();
                    explanation = $"Decimal {input} = Binary {binary}\n\n" +
                                 $"Conversion steps:\n" +
                                 $"1. Divide {input} by 2 repeatedly\n" +
                                 $"2. Read remainders from bottom to top\n" +
                                 $"3. {input}₁₀ = {binary}₂";
                    break;

                case "DecimalToOctal":
                    decimalValue = long.Parse(input);
                    binary = Convert.ToString(decimalValue, 2);
                    octal = Convert.ToString(decimalValue, 8);
                    hex = Convert.ToString(decimalValue, 16).ToUpper();
                    explanation = $"Decimal {input} = Octal {octal}\n\n" +
                                 $"Conversion steps:\n" +
                                 $"1. Divide {input} by 8 repeatedly\n" +
                                 $"2. Read remainders from bottom to top\n" +
                                 $"3. {input}₁₀ = {octal}₈";
                    break;

                case "DecimalToHex":
                    decimalValue = long.Parse(input);
                    binary = Convert.ToString(decimalValue, 2);
                    octal = Convert.ToString(decimalValue, 8);
                    hex = Convert.ToString(decimalValue, 16).ToUpper();
                    explanation = $"Decimal {input} = Hex {hex}\n\n" +
                                 $"Conversion steps:\n" +
                                 $"1. Divide {input} by 16 repeatedly\n" +
                                 $"2. Read remainders (convert 10-15 to A-F)\n" +
                                 $"3. {input}₁₀ = {hex}₁₆";
                    break;

                case "OctalToBinary":
                    decimalValue = Convert.ToInt64(input, 8);
                    binary = Convert.ToString(decimalValue, 2);
                    octal = input;
                    hex = Convert.ToString(decimalValue, 16).ToUpper();
                    explanation = $"Octal {input} = Binary {binary}\n\n" +
                                 $"Conversion steps:\n" +
                                 $"1. Convert octal to decimal: {input} = {decimalValue}\n" +
                                 $"2. Each octal digit represents 3 binary bits\n" +
                                 $"3. Convert decimal {decimalValue} to binary";
                    break;

                case "OctalToDecimal":
                    decimalValue = Convert.ToInt64(input, 8);
                    binary = Convert.ToString(decimalValue, 2);
                    octal = input;
                    hex = Convert.ToString(decimalValue, 16).ToUpper();
                    explanation = $"Octal {input} = Decimal {decimalValue}\n\n" +
                                 $"Conversion steps:\n" +
                                 $"1. Each octal digit represents a power of 8\n" +
                                 $"2. {GetOctalToDecimalExplanation(input)}\n" +
                                 $"3. Sum = {decimalValue}";
                    break;

                case "OctalToHex":
                    decimalValue = Convert.ToInt64(input, 8);
                    binary = Convert.ToString(decimalValue, 2);
                    octal = input;
                    hex = Convert.ToString(decimalValue, 16).ToUpper();
                    explanation = $"Octal {input} = Hex {hex}\n\n" +
                                 $"Conversion steps:\n" +
                                 $"1. Convert octal to decimal: {input} = {decimalValue}\n" +
                                 $"2. Convert decimal {decimalValue} to hex\n" +
                                 $"3. {input}₈ = {hex}₁₆";
                    break;

                case "HexToBinary":
                    decimalValue = Convert.ToInt64(input, 16);
                    binary = Convert.ToString(decimalValue, 2);
                    octal = Convert.ToString(decimalValue, 8);
                    hex = input.ToUpper();
                    explanation = $"Hex {input} = Binary {binary}\n\n" +
                                 $"Conversion steps:\n" +
                                 $"1. Convert hex to decimal: {input} = {decimalValue}\n" +
                                 $"2. Each hex digit represents 4 binary bits\n" +
                                 $"3. Convert decimal {decimalValue} to binary";
                    break;

                case "HexToDecimal":
                    decimalValue = Convert.ToInt64(input, 16);
                    binary = Convert.ToString(decimalValue, 2);
                    octal = Convert.ToString(decimalValue, 8);
                    hex = input.ToUpper();
                    explanation = $"Hex {input} = Decimal {decimalValue}\n\n" +
                                 $"Conversion steps:\n" +
                                 $"1. Each hex digit represents a power of 16\n" +
                                 $"2. {GetHexToDecimalExplanation(input)}\n" +
                                 $"3. Sum = {decimalValue}";
                    break;

                case "HexToOctal":
                    decimalValue = Convert.ToInt64(input, 16);
                    binary = Convert.ToString(decimalValue, 2);
                    octal = Convert.ToString(decimalValue, 8);
                    hex = input.ToUpper();
                    explanation = $"Hex {input} = Octal {octal}\n\n" +
                                 $"Conversion steps:\n" +
                                 $"1. Convert hex to decimal: {input} = {decimalValue}\n" +
                                 $"2. Convert decimal {decimalValue} to octal\n" +
                                 $"3. {input}₁₆ = {octal}₈";
                    break;
            }

            return new ConversionResult(binary, decimalValue.ToString(), octal, hex);
        }

        private string GetBinaryToDecimalExplanation(string binary)
        {
            List<string> parts = new List<string>();
            int length = binary.Length;
            for (int i = 0; i < length; i++)
            {
                if (binary[i] == '1')
                {
                    int power = length - 1 - i;
                    parts.Add($"2^{power} = {Math.Pow(2, power)}");
                }
            }
            return string.Join(" + ", parts);
        }

        private string GetOctalToDecimalExplanation(string octal)
        {
            List<string> parts = new List<string>();
            int length = octal.Length;
            for (int i = 0; i < length; i++)
            {
                int digit = int.Parse(octal[i].ToString());
                if (digit != 0)
                {
                    int power = length - 1 - i;
                    parts.Add($"{digit} × 8^{power} = {digit * Math.Pow(8, power)}");
                }
            }
            return string.Join(" + ", parts);
        }

        private string GetHexToDecimalExplanation(string hex)
        {
            List<string> parts = new List<string>();
            int length = hex.Length;
            string hexUpper = hex.ToUpper();
            for (int i = 0; i < length; i++)
            {
                char c = hexUpper[i];
                int digit = c >= 'A' ? c - 'A' + 10 : int.Parse(c.ToString());
                if (digit != 0)
                {
                    int power = length - 1 - i;
                    parts.Add($"{digit} × 16^{power} = {digit * Math.Pow(16, power)}");
                }
            }
            return string.Join(" + ", parts);
        }

        private void DisplayBits(string binary)
        {
            divBitsDisplay.Controls.Clear();
            foreach (char bit in binary)
            {
                Label lbl = new Label();
                lbl.Text = bit.ToString();
                lbl.CssClass = "bit " + (bit == '1' ? "bit-1" : "bit-0");
                divBitsDisplay.Controls.Add(lbl);
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            pnlError.Visible = true;
        }

        private void SaveToHistory(string entry)
        {
            List<string> history = Session["BinaryHistory"] as List<string> ?? new List<string>();
            history.Insert(0, $"{DateTime.Now.ToString("HH:mm")}: {entry}");
            if (history.Count > 10) history.RemoveAt(10);
            Session["BinaryHistory"] = history;
            UpdateHistoryDisplay();
        }

        private void UpdateHistoryDisplay()
        {
            rptHistory.DataSource = Session["BinaryHistory"] as List<string>;
            rptHistory.DataBind();
        }

        protected void btnUnitConverter_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }

        protected void btnBinaryConverter_Click(object sender, EventArgs e)
        {
            Response.Redirect("BinaryConverter.aspx");
        }

        // Helper class for conversion results
        public class ConversionResult
        {
            public string Binary { get; set; }
            public string Decimal { get; set; }
            public string Octal { get; set; }
            public string Hexadecimal { get; set; }

            public ConversionResult(string binary, string dec, string octal, string hex)
            {
                Binary = binary;
                Decimal = dec;
                Octal = octal;
                Hexadecimal = hex;
            }

            public string GetResultString(string conversionType)
            {
                switch (conversionType)
                {
                    case "BinaryToDecimal": return $"{Binary}₂ = {Decimal}₁₀";
                    case "BinaryToOctal": return $"{Binary}₂ = {Octal}₈";
                    case "BinaryToHex": return $"{Binary}₂ = {Hexadecimal}₁₆";
                    case "DecimalToBinary": return $"{Decimal}₁₀ = {Binary}₂";
                    case "DecimalToOctal": return $"{Decimal}₁₀ = {Octal}₈";
                    case "DecimalToHex": return $"{Decimal}₁₀ = {Hexadecimal}₁₆";
                    case "OctalToBinary": return $"{Octal}₈ = {Binary}₂";
                    case "OctalToDecimal": return $"{Octal}₈ = {Decimal}₁₀";
                    case "OctalToHex": return $"{Octal}₈ = {Hexadecimal}₁₆";
                    case "HexToBinary": return $"{Hexadecimal}₁₆ = {Binary}₂";
                    case "HexToDecimal": return $"{Hexadecimal}₁₆ = {Decimal}₁₀";
                    case "HexToOctal": return $"{Hexadecimal}₁₆ = {Octal}₈";
                    default: return $"{Binary}₂ = {Decimal}₁₀ = {Octal}₈ = {Hexadecimal}₁₆";
                }
            }
        }
    }
}