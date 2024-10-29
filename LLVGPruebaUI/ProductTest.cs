using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LLVG20240907.Tests.UI
{
    [TestClass]
    public class ProductLLVGCreateTest : IDisposable
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string BaseUrl = "https://localhost:7239";

        public ProductLLVGCreateTest()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Window.Maximize();
        }

        [TestMethod]
        public void TestCrearProducto()
        {
            try
            {
                driver.Navigate().GoToUrl($"{BaseUrl}/ProductLLVG");


                // Navegar a la página inicial de productos
                driver.Navigate().GoToUrl($"{BaseUrl}/ProductLLVG/Create");

                // Esperar a que el formulario esté visible y completar los campos
                wait.Until(d => d.FindElement(By.Id("NombreLLVG")))
                    .SendKeys(" de Prueba Automatizada");

                driver.FindElement(By.Id("DescripcionLLVG"))
                    .SendKeys("Descripción de prueba automatizada");

                driver.FindElement(By.Id("PrecioLLVG"))
                    .SendKeys("199.99");

                // Encontrar y enviar el formulario
                var form = driver.FindElement(By.TagName("form"));
                form.Submit();

                // Esperar a que redirija al índice y verificar que aparezca el producto
                wait.Until(d => d.Url.Contains("/ProductLLVG"));

                // Verificar que el producto fue creado buscando en la tabla
                var productCell = wait.Until(d =>
                    d.FindElement(By.XPath("//td[contains(text(), 'Producto de Prueba Automatizada')]")));

                Assert.IsNotNull(productCell,
                    "El producto no aparece en la lista después de ser creado");

                Console.WriteLine("Producto creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la prueba: {ex.Message}");
                TakeScreenshot();
                throw;
            }
        }

        private void TakeScreenshot()
        {
            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var fileName = $"ErrorScreenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                screenshot.SaveAsFile(fileName);
                Console.WriteLine($"Screenshot guardado como {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al tomar screenshot: {ex.Message}");
            }
        }

        public void Dispose()
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }
}