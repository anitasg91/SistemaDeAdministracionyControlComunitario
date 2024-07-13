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

                // Si las credenciales fallan, debe de regresar un error, de lo contrario, la prueba es exitosa
                Assert.IsNotNull(resultado, "Error en el login, el usuario y/o la contraseña son incorrectas.");
               
            }
        }

        private LoginViewModel GetModelLogin() {
            return new LoginViewModel { SessionStart = "1", Password="123456" };
        }

        [Test]

        public void GetGetAuthorization()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var modulos = AppNegocio.GetModulesAllowed(IdUsuario, IdApp);
                Assert.IsTrue(modulos.Any(), "El usuario no tiene modulos asignados.");

                var permisos = AppNegocio.GetPermissionPageByUserApp(IdUsuario, IdApp);
                Assert.IsTrue(permisos.Any(), "El usuario no tiene permisos asignados.");

                var roles = AppNegocio.GetRolesByUserAndAplication(IdUsuario, IdApp);
                Assert.IsTrue(roles.Any(), "El usuario no tiene roles asignados.");

                var manzanas = AppNegocio.GetBlockAsignedToUser(IdUsuario, IdApp);
                Assert.IsTrue(manzanas.Any(), "El usuario no tiene manzanas asignadas.");
            }

        }

        [Test]
        [Ignore("Se crea test GetGetAuthorization para validarlos juntos.")]
        public void GetModules()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetModulesAllowed(IdUsuario, IdApp);
                Assert.IsNotNull(resultado, "El usuario no tiene modulos asignados.");
            }

        }

        [Test]
        [Ignore("Se crea test GetGetAuthorization para validarlos juntos.")]
        public void GetPermissionPageByUserApp()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetPermissionPageByUserApp(IdUsuario, IdApp);
                Assert.IsNotNull(resultado, "El usuario no tiene permisos asignados.");

            }
        }

        [Test]
        [Ignore("Se crea test GetGetAuthorization para validarlos juntos.")]
        public void GetRolesByUserAndAplication()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetRolesByUserAndAplication(IdUsuario, IdApp);
                Assert.IsNotNull(resultado, "El usuario no tiene roles asignados.");
            }
        }

        [Test]
        [Ignore("Se crea test GetGetAuthorization para validarlos juntos.")]
        public void GetBlockAsignedToUser()
        {
            using (ApplicationBusiness AppNegocio = new ApplicationBusiness())
            {
                var resultado = AppNegocio.GetBlockAsignedToUser(IdUsuario, IdApp);
                Assert.IsNotNull(resultado, "El usuario no tiene manzanas asignadas.");

            }
        }


    }
}