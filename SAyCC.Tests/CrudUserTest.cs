using Microsoft.AspNetCore.Mvc;
using SAyCC.Bussiness.Common;
using SAyCC.Bussiness.Login;
using SAyCC.Bussiness.SystemAdmin;
using SAyCC.Entities.Common;
using SAyCC.Entities.Login;
using SistemaAdministracionyControlComunidad.Models;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SAyCC.Tests
{
    public class CrudUserTest
    {
        public int IdUsuario = 1;
        public int IdApp = 1;
        public CrudUserTest()
        {
            DBSet.DBcnn = "Data Source=LEY\\SQLEXPRESS;Initial Catalog=SAyCC;integrated security = true";
        }

        [Test]
        public void InsertNewUserWithWM()
        {
            UserEntity entity = getUserModel(true);
            string usuario = string.Empty;
            string Msg = string.Empty;
            int Id = 0;
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                try
                {
                    string imageUp = "";
                    entity.ImagenUpload = string.IsNullOrEmpty(imageUp) ? null : imageUp;
                    entity.Password = entity.CreateUser ? GlobalBusines.GetSHA256("Tempo123$") : null;
                    entity.IdUsuarioAlta = IdUsuario;
                    entity.IdUsuarioMod = entity.Id == 0 ? new int?() : IdUsuario;
                    usuario = nego.saveUser(entity, ref Id);

                    if (Id > 0 && entity.Medidor != null && entity.Medidor.Count() > 0)
                    {
                        var medidor = nego.GetWaterMeterByIdUser(entity.Id);
                        var eliminar = (from t in medidor where !entity.Medidor.Any(x => x.Id == t.Id) select t).ToList();
                        foreach (var item in eliminar)
                        {
                            nego.UpdateWaterMeterById(item.Id, (int)Enumerador.ModifyTypeWaterMeter.Disassociate, new int?());
                        }
                        foreach (var item in entity.Medidor)
                        {
                            item.IdTitular = Id;
                            nego.SaveWaterMeter(item);
                        }
                    }
                    Assert.IsTrue(!string.IsNullOrEmpty(usuario), "Error al crear el usuario");
                }
                catch (Exception e)
                {
                    if (Id > 0)
                    {
                        var eliminado = nego.DeleteUserForErrorToSave(Id);//Si falló alguna intrucción, se elimina el usuario registrado:
                    }
                    Assert.IsTrue(e == null, "No se pudo crear el usuario. Error: " + e.Message);
                }
            }
        }

        private UserEntity getUserModel(bool includeWM)
        {
            UserEntity entity = new UserEntity()
            {
                Id = 0,
                Nombre = "Saúl",
                APaterno = "Lozano",
                AMaterno = "Acosta",
                Telefono = null,
                Email = null,
                FechaAlta = DateTime.Now,
                IdUsuarioAlta = 1,
                Sexo = true,
                IdManzana = 5,
                FechaNacimiento = Convert.ToDateTime("01/01/1990"),
                SendEmail = false,
                CreateUser = true,
            };

            if (includeWM)
            {
                Random random = new Random();
                int numeroAleatorio = random.Next(100000000, 1000000000);

                entity.Medidor = new List<Entities.WaterSystem.WaterMeterEntity>() {
                    new Entities.WaterSystem.WaterMeterEntity() {
                        Id = 0,
                        Numero = numeroAleatorio.ToString(),
                        FechaAlta = DateTime.Now,
                        LecturaActual = 0,
                        LecturaAnterior = 0,
                        Activo = true,
                        IdManzana = entity.IdManzana
                    }
                };
            }

            return entity;
        }

        [Test]
        public void GetUserAndEditUserWithWM()
        {
            UserEntity entity = getUserModel(true);
            using (LoginBusiness nego = new LoginBusiness())
            {
               int IdNewUser = 36;
                var resultado = nego.GetUser(IdNewUser).FirstOrDefault();// Se obtiene el usuario
                Assert.IsTrue(resultado != null, "Usuario no encontrado.");

                resultado.Email = "saullozano@correo.com";
                resultado.Telefono = "5520369890";

               var editado = nego.UpdateInfoUser(resultado);
                Assert.IsTrue(editado > 0, "El usuario no pudo ser editado.");
            }
        }

        [Test]
        public void DeleteUser()
        {
            int IdUsuarioElimina = 1;
            int IdUsuarioEliminar = 36;
            int Estatus = 0; //Se solicita la eliminación
            using (ApplicationBusiness nego = new ApplicationBusiness())
            {
                try
                {
                    string Motivo = GetMessageStatusValue(Estatus);
                    nego.ChangeStatusUserBySuperAdmin(IdUsuarioEliminar, GetNewStatusValueForSuperAdmin(Estatus), Motivo, IdUsuarioElimina);
                    Assert.True(true);
                }

                catch (Exception e)
                {
                    Assert.IsTrue(e == null, "No se pudo eliminar el usuario. Error: " + e.Message);
                }
            }
        }

        private string GetMessageStatusValue(int Status)
        {
            string mensaje = "";
            string UserName = "Josue Gabriel Pardines Albarrán";
            string MensajeCambioEstatus = "El usuario se ha {0} por solicitud del super admin {1}";
            switch (Status)
            {
                case 2:
                case 8:
                case 9:
                    mensaje = string.Format(MensajeCambioEstatus, "activado", UserName);
                    break;
                case 1:
                case 6:
                case 7:
                    mensaje = string.Format(MensajeCambioEstatus, "desactivado", UserName);
                    break;
                case 0:
                case 3:
                case 4:
                case 5:
                    mensaje = string.Format(MensajeCambioEstatus, "eliminado", UserName);
                    break;
            }
            return mensaje;
        }

        private int GetNewStatusValueForSuperAdmin(int CurrentStatus)
        {
            switch (CurrentStatus)
            {
                case 1:
                case 6:
                case 7:
                    return 2;

                case 2:
                case 8:
                case 9:
                    return 1;

                case 0:
                case 3:
                case 4:
                case 5:
                    return 3;

                default:
                    return CurrentStatus;
            }
        }




    }
}