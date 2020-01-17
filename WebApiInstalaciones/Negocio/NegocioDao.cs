using Entidades;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NegocioDao
    {
        private static string db = ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public static Usuario GetOne(Query q)
        {
            try
            {
                Usuario u = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("Movil_GetUsuario", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = q.user;

                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            u = new Usuario();
                            if (EncriptarClave(q.pass, true) == dr.GetString(1))
                            {
                                u.usuarioId = dr.GetString(0);
                                u.nombre = dr.GetString(2);
                                u.dniCuadrillaId = dr.GetString(3);
                                u.cuadrillaId = dr.GetString(4);
                                u.mensaje = "Go";

                                Filtro f = new Filtro();

                                // Sucursal
                                //SqlCommand cmdS = cn.CreateCommand();
                                //cmdS.CommandTimeout = 0;
                                //cmdS.CommandType = CommandType.StoredProcedure;
                                //cmdS.CommandText = "Movil_GetSucursales";
                                //cmdS.Parameters.Add("@usuario", SqlDbType.VarChar).Value = u.usuarioId;
                                //SqlDataReader drS = cmdS.ExecuteReader();
                                //if (drS.HasRows)
                                //{
                                //    List<Sucursal> sucursales = new List<Sucursal>();
                                //    while (drS.Read())
                                //    {
                                //        sucursales.Add(new Sucursal()
                                //        {
                                //            codigo = drS.GetString(0),
                                //            nombre = drS.GetString(1)
                                //        });
                                //    }
                                //    f.sucursales = sucursales;
                                //}

                                // Areas
                                SqlCommand cmdArea = cn.CreateCommand();
                                cmdArea.CommandTimeout = 0;
                                cmdArea.CommandType = CommandType.StoredProcedure;
                                cmdArea.CommandText = "Movil_GetArea";
                                cmdArea.Parameters.Add("@usuario", SqlDbType.VarChar).Value = u.usuarioId;
                                SqlDataReader drArea = cmdArea.ExecuteReader();
                                if (drArea.HasRows)
                                {
                                    List<Area> area = new List<Area>();
                                    while (drArea.Read())
                                    {
                                        area.Add(new Area()
                                        {
                                            areaId = drArea.GetString(0),
                                            descripcion = drArea.GetString(1),
                                            estado = drArea.GetString(2),
                                            manual = drArea.GetString(3),
                                            medidorEncontrado = drArea.GetString(4),
                                            medidorInstalado = drArea.GetString(5)
                                        });
                                    }
                                    f.areas = area;
                                }

                                // Centro de Costos
                                SqlCommand cmdCentro = cn.CreateCommand();
                                cmdCentro.CommandTimeout = 0;
                                cmdCentro.CommandType = CommandType.StoredProcedure;
                                cmdCentro.CommandText = "Movil_GetCentroCostos";
                                cmdCentro.Parameters.Add("@codigoUsuario", SqlDbType.VarChar).Value = u.usuarioId;

                                SqlDataReader drCentro = cmdCentro.ExecuteReader();
                                if (drCentro.HasRows)
                                {
                                    List<CentroCostos> centros = new List<CentroCostos>();
                                    while (drCentro.Read())
                                    {
                                        CentroCostos c = new CentroCostos();
                                        c.centroId = drCentro.GetString(0);
                                        c.orden = drCentro.GetString(1);
                                        c.descripcion = drCentro.GetString(2);
                                        c.sucursalId = drCentro.GetString(3);
                                        c.nombreSucursal = drCentro.GetString(4);

                                        //SqlCommand cmdCuadrilla = cn.CreateCommand();
                                        //cmdCuadrilla.CommandTimeout = 0;
                                        //cmdCuadrilla.CommandType = CommandType.StoredProcedure;
                                        //cmdCuadrilla.CommandText = "Movil_GetCuadrillas";
                                        //cmdCuadrilla.Parameters.Add("@centroCostoId", SqlDbType.VarChar).Value = centro.orden;
                                        //cmdCuadrilla.Parameters.Add("@codigoUsuario", SqlDbType.VarChar).Value = u.usuarioId;

                                        //SqlDataReader drCuadrilla = cmdCuadrilla.ExecuteReader();
                                        //if (drCuadrilla.HasRows)
                                        //{
                                        //    List<Cuadrilla> cuadrilla = new List<Cuadrilla>();
                                        //    while (drCuadrilla.Read())
                                        //    {
                                        //        cuadrilla.Add(new Cuadrilla()
                                        //        {
                                        //            orden = drCuadrilla.GetString(0),
                                        //            centroId = drCuadrilla.GetString(1),
                                        //            cuadrillaId = drCuadrilla.GetString(2),
                                        //            descripcion = drCuadrilla.GetString(3),
                                        //            dni = drCuadrilla.GetString(4)
                                        //        });
                                        //    }
                                        //    centro.cuadrillas = cuadrilla;
                                        //}
                                        centros.Add(c);
                                    }
                                    f.centros = centros;
                                }
                                u.filtro = f;
                            }
                            else
                            {
                                u.mensaje = "Pass";
                            }
                        }
                    }
                    cn.Close();
                }
                return u;

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static string EncriptarClave(string cExpresion, bool bEncriptarCadena)
        {
            string cResult = "";
            string cPatron = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwwyz";
            string cEncrip = "^çºªæÆöûÿø£Ø×ƒ¬½¼¡«»ÄÅÉêèï7485912360^çºªæÆöûÿø£Ø×ƒ¬½¼¡«»ÄÅÉêèï";


            if (bEncriptarCadena == true)
            {
                cResult = CHRTRAN(cExpresion, cPatron, cEncrip);
            }
            else
            {
                cResult = CHRTRAN(cExpresion, cEncrip, cPatron);
            }

            return cResult;

        }

        public static string CHRTRAN(string cExpresion, string cPatronBase, string cPatronReemplazo)
        {
            string cResult = "";

            int rgChar;
            int nPosReplace;

            for (rgChar = 1; rgChar <= Strings.Len(cExpresion); rgChar++)
            {
                nPosReplace = Strings.InStr(1, cPatronBase, Strings.Mid(cExpresion, rgChar, 1));

                if (nPosReplace == 0)
                {
                    nPosReplace = rgChar;
                    cResult = cResult + Strings.Mid(cExpresion, nPosReplace, 1);
                }
                else
                {
                    if (nPosReplace > cPatronReemplazo.Length)
                    {
                        nPosReplace = rgChar;
                        cResult = cResult + Strings.Mid(cExpresion, nPosReplace, 1);
                    }
                    else
                    {
                        cResult = cResult + Strings.Mid(cPatronReemplazo, nPosReplace, 1);
                    }
                }
            }
            return cResult;
        }

        public static Sincronizar GetSincronizar(Query q)
        {
            try
            {
                Sincronizar s = new Sincronizar();

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    SqlCommand cmdAlm = con.CreateCommand();
                    cmdAlm.CommandTimeout = 0;
                    cmdAlm.CommandType = CommandType.StoredProcedure;
                    cmdAlm.CommandText = "Movil_GetAlmacenes";
                    cmdAlm.Parameters.Add("@sucursal", SqlDbType.VarChar).Value = q.sucursalId;
                    cmdAlm.Parameters.Add("@usuario", SqlDbType.VarChar).Value = q.usuarioId;
                    SqlDataReader drAl = cmdAlm.ExecuteReader();
                    if (drAl.HasRows)
                    {
                        List<Almacen> alm = new List<Almacen>();
                        while (drAl.Read())
                        {
                            alm.Add(new Almacen()
                            {
                                codigo = drAl.GetString(0),
                                tipoCodigo = drAl.GetString(1),
                                sucursalCodigo = drAl.GetString(2),
                                descripcion = drAl.GetString(3),
                                opCodigo = drAl.GetString(4),
                                inventario = drAl.GetString(5),
                                equipamiento = drAl.GetString(6),
                                seraEdelnor = drAl.GetString(7)
                            });
                        }
                        s.almacens = alm;
                    }

                    SqlCommand cmdParametro = con.CreateCommand();
                    cmdParametro.CommandTimeout = 0;
                    cmdParametro.CommandType = CommandType.StoredProcedure;
                    cmdParametro.CommandText = "Movil_GetParametros";
                    SqlDataReader drParametro = cmdParametro.ExecuteReader();
                    if (drParametro.HasRows)
                    {
                        List<Parametro> parametro = new List<Parametro>();
                        while (drParametro.Read())
                        {
                            parametro.Add(new Parametro()
                            {
                                id_Configuracion = drParametro.GetInt32(0),
                                nombre_parametro = drParametro.GetString(1),
                                valor = drParametro.GetInt32(2)
                            });
                        }
                        s.parametros = parametro;
                    }

                    // Parte Diario

                    SqlCommand cmdP = con.CreateCommand();
                    cmdP.CommandTimeout = 0;
                    cmdP.CommandType = CommandType.StoredProcedure;
                    cmdP.CommandText = "Movil_GetParteDiario";
                    cmdP.Parameters.Add("@Area", SqlDbType.VarChar).Value = q.areaId;
                    cmdP.Parameters.Add("@UsuarioId", SqlDbType.VarChar).Value = q.usuarioId;
                    cmdP.Parameters.Add("@CentroCostoId", SqlDbType.VarChar).Value = q.centroCostoId;
                    SqlDataReader drP = cmdP.ExecuteReader();
                    if (drP.HasRows)
                    {
                        List<ParteDiario> parte = new List<ParteDiario>();
                        while (drP.Read())
                        {
                            var p = new ParteDiario();
                            p.parteDiarioId = drP.GetInt32(0);
                            p.identity = drP.GetInt32(0);
                            p.fecha = drP.GetDateTime(1).ToString("dd/MM/yyyy");
                            p.obraTd = drP.GetString(2);
                            p.codigoEstadoPd = drP.GetString(3);
                            p.estadoTd = drP.GetString(4);
                            p.codigoEstadoObra = drP.GetString(5);
                            p.estadoObra = drP.GetString(6);
                            p.descripcion = drP.GetString(7);
                            p.direccion = drP.GetString(8);
                            p.cliente = drP.GetString(9);
                            p.fechaAsignacion = drP.GetDateTime(10).ToString("dd/MM/yyyy");
                            p.fechaVencimiento = drP.GetDateTime(11).ToString("dd/MM/yyyy");
                            p.suministro = drP.GetString(12);
                            p.sed = drP.GetString(13);
                            p.observacion = drP.GetString(14);
                            p.empresaCodigo = drP.GetString(15);
                            p.areaCodigo = drP.GetString(16);
                            p.codigoInterno = drP.GetString(17);
                            p.costoCodigo = drP.GetString(18);
                            p.cuadrillaCodigo = drP.GetString(19);
                            p.dniCuadrilla = drP.GetString(20);
                            p.estadoCodigo = drP.GetString(21);
                            p.usuarioCreacion = drP.GetString(22);
                            p.fechaMovil = drP.GetDateTime(23).ToString("dd/MM/yyyy HH:mm:ss");
                            p.sucursalId = drP.GetString(24);
                            p.latitud = drP.GetString(25);
                            p.longitud = drP.GetString(26);
                            p.coordinadorDni = drP.GetString(27);
                            p.descripcionCoordinador = drP.GetString(28);
                            p.firmaMovil = drP.GetString(29);
                            p.nroFicha = drP.GetString(30);
                            p.tipo = 1;
                            p.estado = 1;
                            p.medencontrado_nro = "";
                            p.medencontrado_marca = "";
                            p.medencontrado_fase = "";
                            p.medencontrado_estado = "";
                            p.medencontrado_modelo = "";
                            p.medinstalado_nro = "";
                            p.medinstalado_marca = "";
                            p.medinstalado_fase = "";
                            p.medinstalado_estado = "";
                            p.medinstalado_modelo = "";

                            // PARTE DIARIO BAREMO

                            SqlCommand cmdPB = con.CreateCommand();
                            cmdPB.CommandTimeout = 0;
                            cmdPB.CommandType = CommandType.StoredProcedure;
                            cmdPB.CommandText = "Movil_GetParteDiarioBaremo";
                            cmdPB.Parameters.Add("@parteDiarioId", SqlDbType.VarChar).Value = p.parteDiarioId;

                            SqlDataReader dmPB = cmdPB.ExecuteReader();
                            if (dmPB.HasRows)
                            {
                                List<RegistroBaremo> rb = new List<RegistroBaremo>();

                                while (dmPB.Read())
                                {
                                    rb.Add(new RegistroBaremo()
                                    {
                                        registroBaremoId = dmPB.GetInt32(0),
                                        identityDetalle = dmPB.GetInt32(0),
                                        parteDiarioId = dmPB.GetInt32(1),
                                        identity = dmPB.GetInt32(1),
                                        codigoBaremo = dmPB.GetString(2),
                                        cantidadMovil = dmPB.GetDecimal(3),
                                        cantidadOk = dmPB.GetDecimal(4),
                                        fecha = dmPB.GetDateTime(5).ToString("dd/MM/yyyy"),
                                        tipo = 1,
                                        descripcion = dmPB.GetString(6),
                                        abreviatura = dmPB.GetString(7),
                                        unidadMedida = dmPB.GetString(8),
                                        estado = p.estadoCodigo
                                    });
                                }
                                p.baremos = rb;
                            }

                            // PARTE DIARIO MATERIALES

                            SqlCommand cmdPM = con.CreateCommand();
                            cmdPM.CommandTimeout = 0;
                            cmdPM.CommandType = CommandType.StoredProcedure;
                            cmdPM.CommandText = "Movil_GetParteDiarioMaterial";
                            cmdPM.Parameters.Add("@parteDiarioId", SqlDbType.VarChar).Value = p.parteDiarioId;

                            SqlDataReader dmPM = cmdPM.ExecuteReader();
                            if (dmPM.HasRows)
                            {
                                List<RegistroMaterial> rm = new List<RegistroMaterial>();

                                while (dmPM.Read())
                                {
                                    rm.Add(new RegistroMaterial()
                                    {
                                        registroMaterialId = dmPM.GetInt32(0),
                                        identityDetalle = dmPM.GetInt32(0),
                                        identity = dmPM.GetInt32(1),
                                        parteDiarioId = dmPM.GetInt32(1),
                                        tipoMaterial = dmPM.GetInt32(2),
                                        almacenId = dmPM.GetString(3),
                                        codigoMaterial = dmPM.GetString(4),
                                        cantidadMovil = dmPM.GetDecimal(5),
                                        cantidadOk = dmPM.GetDecimal(6),
                                        fecha = dmPM.GetDateTime(7).ToString("dd/MM/yyyy"),
                                        tipo = 1,
                                        descripcion = dmPM.GetString(8),
                                        abreviatura = dmPM.GetString(9),
                                        unidadMedida = dmPM.GetString(10),
                                        guiaSalida = dmPM.GetString(11),
                                        nroSerie = dmPM.GetString(12),
                                        estado = p.estadoCodigo
                                    });
                                }
                                p.materiales = rm;
                            }

                            // PARTE DIARIO PHOTOS

                            SqlCommand cmdPP = con.CreateCommand();
                            cmdPP.CommandTimeout = 0;
                            cmdPP.CommandType = CommandType.StoredProcedure;
                            cmdPP.CommandText = "Movil_GetParteDiarioFoto";
                            cmdPP.Parameters.Add("@parteDiarioId", SqlDbType.VarChar).Value = p.parteDiarioId;

                            SqlDataReader dmPP = cmdPP.ExecuteReader();
                            if (dmPP.HasRows)
                            {
                                List<RegistroPhoto> rp = new List<RegistroPhoto>();

                                while (dmPP.Read())
                                {
                                    rp.Add(new RegistroPhoto()
                                    {
                                        registroPhotoId = dmPP.GetInt32(0),
                                        identityFoto = dmPP.GetInt32(0),
                                        identity = dmPP.GetInt32(1),
                                        parteDiarioId = dmPP.GetInt32(1),
                                        nombre = dmPP.GetString(2),
                                        fecha = dmPP.GetDateTime(3).ToString("dd/MM/yyyy"),
                                        tipo = 1
                                    });
                                }
                                p.photos = rp;
                            }
                            parte.Add(p);
                        }
                        s.parteDiarios = parte;
                    }

                    // Baremos

                    SqlCommand cmdB = con.CreateCommand();
                    cmdB.CommandTimeout = 0;
                    cmdB.CommandType = CommandType.StoredProcedure;
                    cmdB.CommandText = "Movil_GetBaremos";
                    cmdB.Parameters.Add("@area", SqlDbType.VarChar).Value = q.areaId;
                    cmdB.Parameters.Add("@cc", SqlDbType.VarChar).Value = q.centroCostoId;
                    cmdB.Parameters.Add("@usuario", SqlDbType.VarChar).Value = q.usuarioId;
                    SqlDataReader drB = cmdB.ExecuteReader();
                    if (drB.HasRows)
                    {
                        List<Baremo> baremo = new List<Baremo>();
                        while (drB.Read())
                        {
                            baremo.Add(new Baremo()
                            {
                                baremoId = drB.GetString(0),
                                descripcion = drB.GetString(1),
                                unidadMedida = drB.GetString(2),
                                abreviatura = drB.GetString(3),
                                actividadId = drB.GetInt32(4)
                            });
                        }
                        s.baremos = baremo;
                    }

                    // Materiales

                    SqlCommand cmdM = con.CreateCommand();
                    cmdM.CommandTimeout = 0;
                    cmdM.CommandType = CommandType.StoredProcedure;
                    cmdM.CommandText = "Movil_GetMateriales";
                    cmdM.Parameters.Add("@area", SqlDbType.VarChar).Value = q.areaId;
                    cmdM.Parameters.Add("@cc", SqlDbType.VarChar).Value = q.centroCostoId;
                    cmdM.Parameters.Add("@usuario", SqlDbType.VarChar).Value = q.usuarioId;
                    SqlDataReader drM = cmdM.ExecuteReader();
                    if (drM.HasRows)
                    {
                        List<Materiales> m = new List<Materiales>();
                        while (drM.Read())
                        {
                            m.Add(new Materiales()
                            {
                                id = drM.GetInt32(0),
                                tipoMaterial = drM.GetString(1),
                                materialId = drM.GetString(2),
                                descripcion = drM.GetString(3),
                                unidadMedida = drM.GetString(4),
                                abreviatura = drM.GetString(5),
                                stock = drM.GetDecimal(6),
                                obra = drM.GetString(7),
                                cc = drM.GetString(8),
                                almacenId = drM.GetString(9),
                                guiaSalida = drM.GetString(10),
                                exigeSerie = drM.GetString(11),
                                tipo = 1,
                                fecha = ""
                            });
                        }
                        s.materiales = m;
                    }

                    // Obras TD

                    SqlCommand cmdO = con.CreateCommand();
                    cmdO.CommandTimeout = 0;
                    cmdO.CommandType = CommandType.StoredProcedure;
                    cmdO.CommandText = "Movil_GetObrasTd";
                    cmdO.Parameters.Add("@usuarioId", SqlDbType.VarChar).Value = q.usuarioId;
                    cmdO.Parameters.Add("@centroCostoId", SqlDbType.VarChar).Value = q.centroCostoId;
                    SqlDataReader drO = cmdO.ExecuteReader();
                    if (drO.HasRows)
                    {
                        List<Obra> o = new List<Obra>();
                        while (drO.Read())
                        {
                            o.Add(new Obra()
                            {
                                obraId = drO.GetString(0),
                                descripcion = drO.GetString(1),
                                estado = drO.GetString(2),
                                direccion = drO.GetString(3),
                                cliente = drO.GetString(4),
                                fechaAsignacion = drO.GetDateTime(5).ToString("dd/MM/yyyy"),
                                fechaVencimiento = drO.GetDateTime(6).ToString("dd/MM/yyyy"),
                                usuarioCreacion = drO.GetString(7)
                            });
                        }
                        s.obras = o;
                    }

                    // Estados

                    SqlCommand cmdE = con.CreateCommand();
                    cmdE.CommandTimeout = 0;
                    cmdE.CommandType = CommandType.StoredProcedure;
                    cmdE.CommandText = "Movil_GetEstados";
                    SqlDataReader drE = cmdE.ExecuteReader();
                    if (drE.HasRows)
                    {
                        List<Estado> e = new List<Estado>();
                        while (drE.Read())
                        {
                            e.Add(new Estado()
                            {
                                codigo = drE.GetString(0),
                                nombre = drE.GetString(1)
                            });
                        }
                        s.estados = e;
                    }

                    // Articulos

                    //SqlCommand cmdA = con.CreateCommand();
                    //cmdA.CommandTimeout = 0;
                    //cmdA.CommandType = CommandType.StoredProcedure;
                    //cmdA.CommandText = "Movil_GetArticulo";
                    //SqlDataReader drA = cmdA.ExecuteReader();
                    //if (drA.HasRows)
                    //{
                    //    List<Articulo> a = new List<Articulo>();
                    //    while (drA.Read())
                    //    {
                    //        a.Add(new Articulo()
                    //        {
                    //            articuloId = drA.GetInt32(0),
                    //            descripcion = drA.GetString(1),
                    //            tipo = drA.GetInt32(2)
                    //        });
                    //    }
                    //    s.articulos = a;
                    //}

                    // Resumen

                    //SqlCommand cmdR = con.CreateCommand();
                    //cmdR.CommandTimeout = 0;
                    //cmdR.CommandType = CommandType.StoredProcedure;
                    //cmdR.CommandText = "Movil_GetResumenParteDiario";
                    //cmdR.Parameters.Add("@Area", SqlDbType.VarChar).Value = q.areaId;
                    //cmdR.Parameters.Add("@UsuarioId", SqlDbType.VarChar).Value = q.usuarioId;
                    //cmdR.Parameters.Add("@CentroCostoId", SqlDbType.VarChar).Value = q.centroCostoId;
                    //cmdR.Parameters.Add("@Filtro", SqlDbType.Int).Value = q.filtro;
                    //cmdR.Parameters.Add("@FechaInicial", SqlDbType.VarChar).Value = q.fechaInicial;
                    //cmdR.Parameters.Add("@FechaFinal", SqlDbType.VarChar).Value = q.fechaFinal;
                    //SqlDataReader drR = cmdR.ExecuteReader();
                    //Resumen r = null;
                    //if (drR.HasRows)
                    //{
                    //    while (drR.Read())
                    //    {
                    //        r = new Resumen()
                    //        {
                    //            resumenId = 1,
                    //            total = drR.GetDecimal(0),
                    //            ejecutas = drR.GetDecimal(1),
                    //            porVencer = drR.GetDecimal(2),
                    //            vencidos = drR.GetDecimal(3),
                    //            pendientes = drR.GetDecimal(4),
                    //            porAvance = drR.GetDecimal(5)
                    //        };
                    //    }
                    //}

                    //s.resumen = r;



                    // Personal

                    SqlCommand cmdPe = con.CreateCommand();
                    cmdPe.CommandTimeout = 0;
                    cmdPe.CommandType = CommandType.StoredProcedure;
                    cmdPe.CommandText = "Movil_getPersonal_CentroCosto";
                    cmdPe.Parameters.Add("@UsuarioId", SqlDbType.VarChar).Value = q.usuarioId;
                    cmdPe.Parameters.Add("@CentroCostoId", SqlDbType.VarChar).Value = q.centroCostoId;
                    SqlDataReader drPe = cmdPe.ExecuteReader();
                    if (drPe.HasRows)
                    {
                        List<Personal> p = new List<Personal>();
                        while (drPe.Read())
                        {
                            p.Add(new Personal()
                            {
                                personalId = drPe.GetInt32(0),
                                empresaId = drPe.GetInt32(1),
                                nroDocumento = drPe.GetString(2),
                                apellido = drPe.GetString(3),
                                nombre = drPe.GetString(4)
                            });
                        }
                        s.personals = p;
                    }

                    SqlCommand cmdCo = con.CreateCommand();
                    cmdCo.CommandTimeout = 0;
                    cmdCo.CommandType = CommandType.StoredProcedure;
                    cmdCo.CommandText = "Movil_GetUsuario_Coordinador";
                    cmdCo.Parameters.Add("@Codigo_OP", SqlDbType.VarChar).Value = q.centroCostoId;
                    SqlDataReader drCo = cmdCo.ExecuteReader();
                    if (drCo.HasRows)
                    {
                        List<Coordinador> c = new List<Coordinador>();
                        while (drCo.Read())
                        {
                            c.Add(new Coordinador()
                            {
                                codigo = drCo.GetString(0),
                                nombre = drCo.GetString(1)
                            });
                        }
                        s.coordinadors = c;
                    }

                    SqlCommand cmdTD = con.CreateCommand();
                    cmdTD.CommandTimeout = 0;
                    cmdTD.CommandType = CommandType.StoredProcedure;
                    cmdTD.CommandText = "Movil_GetTipoDevolucion";
                    SqlDataReader drTD = cmdTD.ExecuteReader();
                    if (drTD.HasRows)
                    {
                        List<TipoDevolucion> t = new List<TipoDevolucion>();
                        while (drTD.Read())
                        {
                            t.Add(new TipoDevolucion()
                            {
                                tipo = drTD.GetInt32(0),
                                descripcion = drTD.GetString(1),
                                estado = drTD.GetInt32(2)
                            });
                        }
                        s.devoluciones = t;
                    }

                    SqlCommand cmdA = con.CreateCommand();
                    cmdA.CommandTimeout = 0;
                    cmdA.CommandType = CommandType.StoredProcedure;
                    cmdA.CommandText = "Movil_GetActividad";
                    SqlDataReader drA = cmdA.ExecuteReader();
                    if (drA.HasRows)
                    {
                        List<Actividad> a = new List<Actividad>();
                        while (drA.Read())
                        {
                            a.Add(new Actividad()
                            {
                                actividadId = drA.GetInt32(0),
                                descripcion = drA.GetString(1)
                            });
                        }
                        s.actividades = a;
                    }

                    // MEDIDOR

                    SqlCommand cmdMe = con.CreateCommand();
                    cmdMe.CommandTimeout = 0;
                    cmdMe.CommandType = CommandType.StoredProcedure;
                    cmdMe.CommandText = "Movil_GetMateriales_Series";
                    cmdMe.Parameters.Add("@area", SqlDbType.VarChar).Value = q.areaId;
                    cmdMe.Parameters.Add("@cc", SqlDbType.VarChar).Value = q.centroCostoId;
                    cmdMe.Parameters.Add("@usuario", SqlDbType.VarChar).Value = q.usuarioId;
                    SqlDataReader drMe = cmdMe.ExecuteReader();
                    if (drMe.HasRows)
                    {
                        List<Medidor> m = new List<Medidor>();
                        while (drMe.Read())
                        {
                            m.Add(new Medidor()
                            {
                                medidorId = drMe.GetString(0),
                                sucursalCodigo = drMe.GetString(1),
                                almacenCodigo = drMe.GetString(2),
                                empleadoDni = drMe.GetString(3),
                                guiaNumero = drMe.GetString(4),
                                articuloCodigo = drMe.GetString(5)
                            });
                        }
                        s.medidores = m;
                    }
                    con.Close();
                }
                return s;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // Guardar Registro Parte Diario
        public static Mensaje SaveRegistro(ParteDiario p)
        {
            try
            {
                Mensaje m = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    // General
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_SaveParteDiario";
                    cmd.Parameters.Add("@parteDiarioId", SqlDbType.Int).Value = p.identity;
                    cmd.Parameters.Add("@tipo", SqlDbType.Int).Value = p.tipo;
                    cmd.Parameters.Add("@pub_empr_codigo", SqlDbType.VarChar).Value = p.empresaCodigo;
                    cmd.Parameters.Add("@pub_area_codigo", SqlDbType.VarChar).Value = p.areaCodigo;
                    cmd.Parameters.Add("@partediario_obratd", SqlDbType.VarChar).Value = p.obraTd;
                    cmd.Parameters.Add("@codigo_interno", SqlDbType.VarChar).Value = p.codigoInterno;
                    cmd.Parameters.Add("@partediario_fecha", SqlDbType.VarChar).Value = p.fecha;
                    cmd.Parameters.Add("@ges_ordt_codigo", SqlDbType.VarChar).Value = p.costoCodigo;
                    cmd.Parameters.Add("@ges_cuad_codigo", SqlDbType.VarChar).Value = p.cuadrillaCodigo;
                    cmd.Parameters.Add("@ges_dni_cuadrilla", SqlDbType.VarChar).Value = p.dniCuadrilla;
                    cmd.Parameters.Add("@partediario_suministro", SqlDbType.VarChar).Value = p.suministro;
                    cmd.Parameters.Add("@partediario_sed", SqlDbType.VarChar).Value = p.sed;
                    cmd.Parameters.Add("@partediario_nroFicha", SqlDbType.VarChar).Value = p.nroFicha;
                    cmd.Parameters.Add("@partediario_obs", SqlDbType.VarChar).Value = p.observacion;
                    cmd.Parameters.Add("@partediario_medencontrado_nro", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@partediario_medencontrado_marca", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@partediario_medencontrado_fase", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@partediario_medencontrado_estado", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@partediario_medencontrado_modelo", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@partediario_medinstalado_nro", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@partediario_medinstalado_marca", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@partediario_medinstalado_fase", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@partediario_medinstalado_estado", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@partediario_medinstalado_modelo", SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@partediario_latitud", SqlDbType.VarChar).Value = p.latitud;
                    cmd.Parameters.Add("@partediario_longitud", SqlDbType.VarChar).Value = p.longitud;
                    cmd.Parameters.Add("@pub_esta_codigo", SqlDbType.VarChar).Value = "121";
                    cmd.Parameters.Add("@partediario_usucrea", SqlDbType.VarChar).Value = p.usuarioCreacion;
                    cmd.Parameters.Add("@partediario_usumodi", SqlDbType.VarChar).Value = p.usuarioCreacion;
                    cmd.Parameters.Add("@partediario_fechamovil", SqlDbType.VarChar).Value = p.fechaMovil;
                    cmd.Parameters.Add("@firma_Movil", SqlDbType.VarChar).Value = p.firmaMovil;
                    cmd.Parameters.Add("@Ges_DNI_Cordinador", SqlDbType.VarChar).Value = p.coordinadorDni;
                    cmd.Parameters.Add("@sucursal", SqlDbType.VarChar).Value = p.sucursalId;

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        m = new Mensaje();

                        while (dr.Read())
                        {

                            m.mensaje = "Mensaje Enviado";
                            m.codigoBase = p.parteDiarioId;
                            m.codigoRetorno = dr.GetInt32(0);

                            foreach (var b in p.baremos)
                            {
                                SqlCommand cmdB = con.CreateCommand();
                                cmdB.CommandTimeout = 0;
                                cmdB.CommandType = CommandType.StoredProcedure;
                                cmdB.CommandText = "Movil_SaveParteDiarioBaremo";
                                cmdB.Parameters.Add("@id_partediario", SqlDbType.Int).Value = m.codigoRetorno;
                                cmdB.Parameters.Add("@id_actividad", SqlDbType.Int).Value = b.actividadId;
                                cmdB.Parameters.Add("@codigobaremo", SqlDbType.VarChar).Value = b.codigoBaremo;
                                cmdB.Parameters.Add("@partediario_bar_cantidadmovil", SqlDbType.Decimal).Value = b.cantidadMovil;
                                cmdB.Parameters.Add("@partediario_bar_cantidadok", SqlDbType.Decimal).Value = b.cantidadOk;
                                cmdB.Parameters.Add("@partediario_fechamovil", SqlDbType.VarChar).Value = b.fecha;
                                cmdB.Parameters.Add("@tipo", SqlDbType.Int).Value = b.tipo;
                                cmdB.ExecuteNonQuery();
                            }

                            foreach (var ma in p.materiales)
                            {
                                SqlCommand cmdM = con.CreateCommand();
                                cmdM.CommandTimeout = 0;
                                cmdM.CommandType = CommandType.StoredProcedure;
                                cmdM.CommandText = "Movil_SaveParteDiarioMaterial";
                                cmdM.Parameters.Add("@id_partediario", SqlDbType.Int).Value = m.codigoRetorno;
                                cmdM.Parameters.Add("@partediario_tipomaterial", SqlDbType.Int).Value = ma.tipoMaterial;
                                cmdM.Parameters.Add("@almacenId", SqlDbType.VarChar).Value = ma.almacenId;
                                cmdM.Parameters.Add("@codigo_material", SqlDbType.VarChar).Value = ma.codigoMaterial;
                                cmdM.Parameters.Add("@partediario_mat_cantidadmovil", SqlDbType.Decimal).Value = ma.cantidadMovil;
                                cmdM.Parameters.Add("@partediario_mat_cantidadok", SqlDbType.Decimal).Value = ma.cantidadOk;
                                cmdM.Parameters.Add("@partediario_fechamovil", SqlDbType.VarChar).Value = ma.fecha;
                                cmdM.Parameters.Add("@tipo", SqlDbType.Int).Value = ma.tipo;
                                cmdM.Parameters.Add("@guiaSalida", SqlDbType.VarChar).Value = ma.guiaSalida;
                                cmdM.Parameters.Add("@nroSerie", SqlDbType.VarChar).Value = ma.nroSerie;
                                cmdM.ExecuteNonQuery();
                            }

                            foreach (var f in p.photos)
                            {
                                SqlCommand cmdF = con.CreateCommand();
                                cmdF.CommandTimeout = 0;
                                cmdF.CommandType = CommandType.StoredProcedure;
                                cmdF.CommandText = "Movil_SaveParteDiarioPhoto";
                                cmdF.Parameters.Add("@id_partediario", SqlDbType.Int).Value = m.codigoRetorno;
                                cmdF.Parameters.Add("@partediario_foto_url", SqlDbType.VarChar).Value = f.nombre;
                                cmdF.Parameters.Add("@partediario_fechamovil", SqlDbType.VarChar).Value = f.fecha;
                                cmdF.Parameters.Add("@tipo", SqlDbType.Int).Value = f.tipo;
                                cmdF.ExecuteNonQuery();
                            }
                        }
                    }

                    con.Close();
                }
                return m;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // Guardar Solicitud Cabecera
        public static Mensaje SaveRegistroGeneral(Solicitud s)
        {
            try
            {
                Mensaje mensaje = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    // General
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_SaveSolicitud";
                    cmd.Parameters.Add("@filtro", SqlDbType.Int).Value = s.filtro;
                    cmd.Parameters.Add("@tipo", SqlDbType.Int).Value = s.tipo;
                    cmd.Parameters.Add("@Id_Solicitud", SqlDbType.Int).Value = s.identity;
                    cmd.Parameters.Add("@id_tipmaterialsol", SqlDbType.Int).Value = s.tipoMaterialSol;
                    cmd.Parameters.Add("@tipoSolicitudId", SqlDbType.Int).Value = s.tipoSolicitudId;
                    cmd.Parameters.Add("@tipomaterial", SqlDbType.Int).Value = s.tipoMaterial;
                    cmd.Parameters.Add("@nro_interno", SqlDbType.VarChar).Value = s.nroInterno;
                    cmd.Parameters.Add("@fechaatencion_solicitud", SqlDbType.VarChar).Value = s.fechaAtencion;
                    cmd.Parameters.Add("@solicitud_obratd", SqlDbType.VarChar).Value = s.obraTd;
                    cmd.Parameters.Add("@obs_solicitud", SqlDbType.VarChar).Value = s.observacion;
                    cmd.Parameters.Add("@ges_ordt_codigo", SqlDbType.VarChar).Value = s.centroCosto;
                    cmd.Parameters.Add("@ges_cuad_codigo", SqlDbType.VarChar).Value = s.cuadrillaCodigo;
                    cmd.Parameters.Add("@ges_dni_cuadrilla", SqlDbType.VarChar).Value = s.dniCuadrilla;
                    cmd.Parameters.Add("@pub_esta_codigo", SqlDbType.VarChar).Value = s.pubEstadoCodigo;
                    cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = s.usuario;
                    cmd.Parameters.Add("@solicitud_fechamovil", SqlDbType.VarChar).Value = s.fechaAsignacion;
                    cmd.Parameters.Add("@ges_dni_coordinador", SqlDbType.VarChar).Value = s.dniCoordinador;
                    cmd.Parameters.Add("@ges_dni_personal", SqlDbType.VarChar).Value = s.dniPersonal;
                    cmd.Parameters.Add("@sucursal", SqlDbType.VarChar).Value = s.sucursalId;

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        mensaje = new Mensaje();

                        while (dr.Read())
                        {
                            mensaje.mensaje = s.tipo == 1 ? "Registro Actualizado" : "Registro Guardado";
                            mensaje.codigoBase = s.solicitudId;
                            mensaje.codigoRetorno = dr.GetInt32(0);
                        }
                    }

                    con.Close();
                }
                return mensaje;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // Guardar Solicitud Detalle
        public static Mensaje SaveRegistroDetalle(RegistroMaterialSolicitud r)
        {
            try
            {
                Mensaje mensaje = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    // General
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_SaveSolicitudDetalle";
                    cmd.Parameters.Add("@solicitudMaterial", SqlDbType.Int).Value = r.identityDetalle;
                    cmd.Parameters.Add("@filtro", SqlDbType.Int).Value = r.filtro;
                    cmd.Parameters.Add("@id_solicitud", SqlDbType.Int).Value = r.identity;
                    cmd.Parameters.Add("@id_tipmaterialsol", SqlDbType.Int).Value = r.tipoMaterial;
                    cmd.Parameters.Add("@codigo_material", SqlDbType.VarChar).Value = r.codigoMaterial;
                    cmd.Parameters.Add("@solicitud_mat_cantidadmovil", SqlDbType.Decimal).Value = r.cantidadMovil;
                    cmd.Parameters.Add("@solicitud_mat_cantidadok", SqlDbType.Decimal).Value = r.cantidadOk;
                    cmd.Parameters.Add("@solicitud_fechamovil", SqlDbType.VarChar).Value = r.fecha;
                    cmd.Parameters.Add("@tipo", SqlDbType.Int).Value = r.tipo;
                    cmd.Parameters.Add("@almacenId", SqlDbType.VarChar).Value = r.almacenId;
                    cmd.Parameters.Add("@usuarioId", SqlDbType.VarChar).Value = r.usuarioId;
                    cmd.Parameters.Add("@GuiaSalida", SqlDbType.VarChar).Value = r.guiaSalida;
                    cmd.Parameters.Add("@Stock", SqlDbType.Decimal).Value = 0;

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        mensaje = new Mensaje();

                        while (dr.Read())
                        {
                            mensaje.codigoBase = r.registroMaterialId;
                            mensaje.codigoRetorno = dr.GetInt32(0);
                            mensaje.mensaje = dr.GetString(1);
                        }
                    }

                    con.Close();
                }
                return mensaje;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Mensaje SaveRegistroPhoto(RegistroPhoto r)
        {
            try
            {
                Mensaje mensaje = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    // General
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_SaveSolicitudFoto";
                    cmd.Parameters.Add("@id_solicitudFoto", SqlDbType.Int).Value = r.identityFoto;
                    cmd.Parameters.Add("@id_solicitud", SqlDbType.Int).Value = r.identity;
                    cmd.Parameters.Add("@solicitud_foto_nombre", SqlDbType.VarChar).Value = r.nombre;
                    cmd.Parameters.Add("@solicitud_fechamovil", SqlDbType.VarChar).Value = r.fecha;
                    cmd.Parameters.Add("@tipo", SqlDbType.Int).Value = r.tipo;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        mensaje = new Mensaje();
                        while (dr.Read())
                        {
                            if (r.tipo == 0)
                            {
                                mensaje.mensaje = "Foto Guardada";
                                mensaje.codigoBase = r.registroPhotoId;
                                mensaje.codigoRetorno = dr.GetInt32(0);
                            }
                            else
                            {
                                mensaje.mensaje = "Foto Eliminada";
                                mensaje.codigoBase = r.registroPhotoId;
                            }
                        }
                    }

                    con.Close();
                }
                return mensaje;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // Stock de Materiales
        public static List<Materiales> GetStockMaterial(Query q)
        {
            try
            {
                List<Materiales> materiales = null;
                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_GesStock_Materiales";
                    cmd.Parameters.Add("@usuarioId", SqlDbType.VarChar).Value = q.usuarioId;
                    cmd.Parameters.Add("@tipoSolicitud", SqlDbType.VarChar).Value = q.tipoSolicitud;
                    cmd.Parameters.Add("@tipoMaterial", SqlDbType.VarChar).Value = q.tipoMaterialSolicitud;
                    cmd.Parameters.Add("@almacenId", SqlDbType.VarChar).Value = q.almacenId;
                    cmd.Parameters.Add("@centroCosto", SqlDbType.VarChar).Value = q.centroCostoId;
                    cmd.Parameters.Add("@filtro", SqlDbType.Int).Value = q.filtro;
                    cmd.Parameters.Add("@codigoArticulo", SqlDbType.VarChar).Value = q.codigoArticulo;
                    cmd.Parameters.Add("@pageIndex", SqlDbType.Int).Value = q.pageIndex;
                    cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = q.pageSize;
                    cmd.Parameters.Add("@search", SqlDbType.VarChar).Value = q.search;
                    cmd.Parameters.Add("@Obra", SqlDbType.VarChar).Value = q.obraId;
                    cmd.Parameters.Add("@dniPersonal", SqlDbType.VarChar).Value = q.personalDni;

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        materiales = new List<Materiales>();
                        while (dr.Read())
                        {
                            materiales.Add(new Materiales()
                            {
                                materialId = dr.GetString(0),
                                descripcion = dr.GetString(1),
                                unidadMedida = dr.GetString(2),
                                abreviatura = dr.GetString(3),
                                stock = Convert.ToDecimal(dr.GetDecimal(4)),
                                fecha = dr.GetString(5),
                                tipo = 2
                            });
                        }
                    }
                    con.Close();
                }
                return materiales;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // Get Solicitud
        public static List<Solicitud> GetSolicitudes(Query q)
        {
            try
            {
                List<Solicitud> s = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    // Solicitud
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_GetSolicitudes";
                    cmd.Parameters.Add("@usuarioId", SqlDbType.VarChar).Value = q.usuarioId;
                    cmd.Parameters.Add("@cuadrilla", SqlDbType.VarChar).Value = q.cuadrillaId;
                    cmd.Parameters.Add("@centroCostoId", SqlDbType.VarChar).Value = q.centroCostoId;
                    cmd.Parameters.Add("@tipoSolicitud", SqlDbType.Int).Value = q.tipoSolicitud;
                    cmd.Parameters.Add("@tipoMaterialSolicitud", SqlDbType.Int).Value = q.tipoMaterialSolicitud;
                    cmd.Parameters.Add("@fechaRegistro", SqlDbType.VarChar).Value = q.fechaRegistro;
                    cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = q.estado;
                    cmd.Parameters.Add("@filtro", SqlDbType.VarChar).Value = q.filtro;
                    cmd.Parameters.Add("@pageIndex", SqlDbType.Int).Value = q.pageIndex;
                    cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = q.pageSize;
                    cmd.Parameters.Add("@search", SqlDbType.VarChar).Value = q.search;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        s = new List<Solicitud>();

                        while (dr.Read())
                        {
                            var o = new Solicitud();
                            o.solicitudId = dr.GetInt32(0);
                            o.identity = dr.GetInt32(0);
                            o.tipoMaterialSol = dr.GetInt32(1);
                            o.filtro = dr.GetInt32(1);
                            o.tipoSolicitudId = dr.GetInt32(2);
                            o.tipoMaterial = dr.GetInt32(3);
                            o.nombreTipoMaterial = dr.GetString(4);
                            o.nroInterno = dr.GetString(5);
                            o.fechaAtencion = dr.GetDateTime(6).ToString("dd/MM/yyyy");
                            o.obraTd = dr.GetString(7);
                            o.codigoEstadoSol = dr.GetString(8);
                            o.estadoSol = dr.GetString(9);
                            o.codigoEstadoObra = dr.GetString(10);
                            o.estadoObra = dr.GetString(11);
                            o.descripcionObra = dr.GetString(12);
                            o.direccionObra = dr.GetString(13);
                            o.clienteObra = dr.GetString(14);
                            o.fechaAsignacion = dr.GetDateTime(15).ToString("dd/MM/yyyy");
                            o.fechaVencimiento = dr.GetDateTime(16).ToString("dd/MM/yyyy");
                            o.observacion = dr.GetString(17);
                            o.dniCuadrilla = dr.GetString(18);
                            o.centroCosto = dr.GetString(19);
                            o.pubEstadoCodigo = dr.GetString(20);
                            o.numeroGuia = dr.GetString(21);
                            o.cuadrillaCodigo = "";
                            o.usuario = dr.GetString(22);
                            o.dniCoordinador = dr.GetString(23);
                            o.nombreCoordinador = dr.GetString(24);
                            o.dniPersonal = dr.GetString(25);
                            o.nombrePersonal = dr.GetString(26);
                            o.sucursalId = dr.GetString(27);
                            o.tipo = 1;

                            SqlCommand cmdSM = con.CreateCommand();
                            cmdSM.CommandTimeout = 0;
                            cmdSM.CommandType = CommandType.StoredProcedure;
                            cmdSM.CommandText = "Movil_GetSolicitudDetalleById";
                            cmdSM.Parameters.Add("@solicitudId", SqlDbType.VarChar).Value = o.solicitudId;

                            SqlDataReader drSM = cmdSM.ExecuteReader();
                            if (drSM.HasRows)
                            {
                                List<RegistroMaterialSolicitud> rr = new List<RegistroMaterialSolicitud>();

                                while (drSM.Read())
                                {
                                    rr.Add(new RegistroMaterialSolicitud()
                                    {
                                        registroMaterialId = drSM.GetInt32(0),
                                        identityDetalle = drSM.GetInt32(0),
                                        identity = drSM.GetInt32(1),
                                        solicitudId = drSM.GetInt32(1),
                                        tipoMaterial = drSM.GetInt32(2),
                                        codigoMaterial = drSM.GetString(3),
                                        cantidadMovil = drSM.GetDecimal(4),
                                        cantidadOk = drSM.GetDecimal(5),
                                        fecha = drSM.GetDateTime(6).ToString("dd/MM/yyyy"),
                                        tipo = 1,
                                        filtro = drSM.GetInt32(2),
                                        descripcion = drSM.GetString(7),
                                        abreviatura = drSM.GetString(8),
                                        unidadMedida = drSM.GetString(9),
                                        almacenId = drSM.GetString(10),
                                        usuarioId = o.usuario,
                                        guiaSalida = drSM.GetString(11),
                                        tipoSolicitudId = o.tipoSolicitudId,
                                        cantidadAprobada = drSM.GetDecimal(12),
                                        estado = o.pubEstadoCodigo
                                    });
                                }
                                o.materiales = rr;
                            }

                            // SOLICITUD PHOTO

                            SqlCommand cmdSF = con.CreateCommand();
                            cmdSF.CommandTimeout = 0;
                            cmdSF.CommandType = CommandType.StoredProcedure;
                            cmdSF.CommandText = "Movil_GetSolicitudFotoById";
                            cmdSF.Parameters.Add("@solicitudId", SqlDbType.VarChar).Value = o.solicitudId;
                            SqlDataReader drSF = cmdSF.ExecuteReader();
                            if (drSF.HasRows)
                            {
                                List<RegistroPhotoSolicitud> f = new List<RegistroPhotoSolicitud>();

                                while (drSF.Read())
                                {
                                    f.Add(new RegistroPhotoSolicitud()
                                    {
                                        registroPhotoId = drSF.GetInt32(0),
                                        identityFoto = drSF.GetInt32(0),
                                        identity = drSF.GetInt32(1),
                                        solicitudId = drSF.GetInt32(1),
                                        nombre = drSF.GetString(2),
                                        fecha = drSF.GetDateTime(3).ToString("dd/MM/yyyy"),
                                        tipo = 1
                                    });

                                }
                                o.photos = f;
                            }

                            s.Add(o);
                        }
                    }
                    con.Close();
                }
                return s;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        // Aprobation 

        public static Mensaje GetAprobation(Query q)
        {
            try
            {
                Mensaje m = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_GetAprobacion";
                    cmd.Parameters.Add("@solicitudId", SqlDbType.VarChar).Value = q.solicitudId;
                    cmd.Parameters.Add("@estado", SqlDbType.VarChar).Value = q.estado;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        m = new Mensaje();
                        while (dr.Read())
                        {
                            m.mensaje = dr.GetString(0);
                        }
                    }
                    con.Close();
                }
                return m;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // SERVICIOS

        public static Mensaje SaveEstadoMovil(EstadoMovil e)
        {
            try
            {
                Mensaje m = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Movil_SaveEstadoCelular";
                    cmd.Parameters.Add("@usuarioId", SqlDbType.VarChar).Value = e.usuarioId;
                    cmd.Parameters.Add("@gpsActivo", SqlDbType.Bit).Value = e.gpsActivo;
                    cmd.Parameters.Add("@estadoBateria", SqlDbType.Int).Value = e.estadoBateria;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                    cmd.Parameters.Add("@modoAvion", SqlDbType.Int).Value = e.modoAvion;
                    cmd.Parameters.Add("@planDatos", SqlDbType.Bit).Value = e.planDatos;
                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigoBase = 1,
                            mensaje = "Enviado"
                        };
                    }

                    cn.Close();
                }

                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Mensaje SaveOperarioGps(EstadoOperario e)
        {
            try
            {
                Mensaje m = null;

                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Movil_SaveGps";
                    cmd.Parameters.Add("@usuarioId", SqlDbType.VarChar).Value = e.usuarioId;
                    cmd.Parameters.Add("@latitud", SqlDbType.VarChar).Value = e.latitud;
                    cmd.Parameters.Add("@longitud", SqlDbType.VarChar).Value = e.longitud;
                    cmd.Parameters.Add("@fechaGPD", SqlDbType.VarChar).Value = e.fechaGPD;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;

                    int a = cmd.ExecuteNonQuery();

                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigoBase = 1,
                            mensaje = "Enviado"
                        };
                    }

                    cn.Close();
                }

                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

