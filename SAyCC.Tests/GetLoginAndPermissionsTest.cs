using Microsoft.AspNetCore.Mvc;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.Login;
using SAyCC.Bussiness.SystemAdmin;
using SistemaAdministracionyControlComunidad.Models;

namespace SAyCC.Tests
{
    public class GetLoginAndPermissionsTest
    {
        public int IdUsuario = 1;
        public int IdApp = 1;
        public GetLoginAndPermissionsTest()
        {
            DBSet.DBcnn = "Data Source=LEY\\SQLEXPRESS;Initial Catalog=SAyCC;integrated security = true";
        }

        [Test]
        public void GetLoginUser()
        {
            LoginViewModel model = GetModelLogin();
            string Password = GlobalBusines.GetSHA256(model.Password);
            string User = model.SessionStart;
            using (LoginBusiness nego = new LoginBusiness())
            {
                var resultado = nego.AllowAccess(User, Password).FirstOrDefault();
                Assert.IsTrue(resultado != null, "Login exitoso para usuario activo y contraseña correcta");
                //Assert.IsFalse(resultado, "Se esperaba que el login fallara para usuario inactivo");
            }
        }

        private LoginViewModel GetModelLogin() {
            return new LoginViewModel { SessionStart = "1", Password="123456" };
        }

        [Test]
        public void GetModules()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetModulesAllowed(IdUsuario, IdApp);
                Assert.IsTrue(resultado.Any());
            }

        }

        [Test]
        public void GetPermissionPageByUserApp()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetPermissionPageByUserApp(IdUsuario, IdApp);
                Assert.IsTrue(resultado.Any());
            }
        }

        [Test]
        public void GetRolesByUserAndAplication()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetRolesByUserAndAplication(IdUsuario, IdApp);
                Assert.IsTrue(resultado.Any());
            }
        }

        [Test]
        public void GetBlockAsignedToUser()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetBlockAsignedToUser(IdUsuario, IdApp);
                Assert.IsTrue(resultado.Any());
            }
        }

       
    }
}