using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fluid;
using Newtonsoft.Json.Linq;

namespace LiquidateIt
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        private readonly FluidParser _fluidParser = new();

        private async Task<string> Render(object model)
        {
            var context = new TemplateContext(model);

            var fluidTemplate = GetFluidTemplate(templateTextBox.Text);

            return await fluidTemplate.RenderAsync(context);
        }

        // NOTE:
        // For now the fluid template is not cached and reused since the serving endpoint which called to this method is cached,
        // but if it will be needed - the Fluid template can be cached
        private IFluidTemplate GetFluidTemplate(string htmlTemplate)
        {
            if (!_fluidParser.TryParse(htmlTemplate, out var fluidTemplate, out var error))
            {
                throw new InvalidOperationException("Error in the HTML template parsing");
            }

            return fluidTemplate;
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                outputTextBox.Text = await Render(JObject.Parse(inputTextBox.Text));
            }
            catch (Exception exception)
            {
                errorLabel.Text = exception.Message;
            }
        }
    }
}
