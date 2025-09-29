using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using PagosIntermex;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;
using Font = System.Drawing.Font;

namespace PagosIntermex.FORMS.REPORTES
{
    public partial class F_REP_PROGRAMACION : Form
    {
        private DataTable dtReporte;
        private C_ConexionSQL conn_sql;
        private C_REGISTROSWINDOWS registros;
        private int? doctoIdFiltro = null; // Para filtrar programación específica

        // Constructor sin parámetros
        public F_REP_PROGRAMACION()
        {
            InitializeComponent();
            InicializarFormulario();
            InicializarConexiones();
            CargarProgramaciones();
        }

        // Constructor con parámetro para filtrar programación específica
        public F_REP_PROGRAMACION(int doctoPrId) : this()
        {
            doctoIdFiltro = doctoPrId;
            chkTodasProgramaciones.Checked = false;
            cmbProgramacion.Enabled = false;

            // Generar automáticamente el reporte filtrado
            this.Load += (s, e) => {
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 500; // 500ms delay
                timer.Tick += (sender, args) => {
                    timer.Stop();
                    timer.Dispose();
                    BtnGenerar_Click(null, null);
                };
                timer.Start();
            };
        }

        private void InicializarFormulario()
        {
            // Configuración adicional del DataGridView que no está en el designer
            dgvReporte.CellFormatting += DgvReporte_CellFormatting;
            dgvReporte.RowPostPaint += DgvReporte_RowPostPaint;

            // Configurar scroll horizontal
            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvReporte.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            dgvReporte.AllowUserToResizeColumns = true;

            // Configurar status strip
            toolStripStatusLabel1.Text = "Listo para generar reporte";

            // Evento del form
            this.Load += F_REP_PROGRAMACION_Load;
        }

        private void InicializarConexiones()
        {
            // Inicializar conexión SQL
            conn_sql = new C_ConexionSQL();

            // Inicializar registros de Windows
            registros = new C_REGISTROSWINDOWS();
            registros.LeerRegistros(false);
        }

        private void F_REP_PROGRAMACION_Load(object sender, EventArgs e)
        {
            if (doctoIdFiltro.HasValue)
            {
                toolStripStatusLabel1.Text = $"Mostrando programación específica: {doctoIdFiltro.Value}";
                this.Text += $" - Programación #{doctoIdFiltro.Value}";
            }
            else
            {
                toolStripStatusLabel1.Text = "Formulario cargado exitosamente";
            }
        }

        private void DgvReporte_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Formateo adicional de celdas
            if (dgvReporte.Columns[e.ColumnIndex].Name.Contains("Importe") ||
                dgvReporte.Columns[e.ColumnIndex].Name.Contains("Precio") ||
                dgvReporte.Columns[e.ColumnIndex].Name.Contains("Subtotal"))
            {
                if (e.Value != null && decimal.TryParse(e.Value.ToString().Replace("$", "").Replace(",", ""), out decimal valor))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
            }

            // Formatear fechas
            if (dgvReporte.Columns[e.ColumnIndex].Name.Contains("Fecha"))
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Formatear números
            if (dgvReporte.Columns[e.ColumnIndex].Name == "Cantidad")
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        // Método para dibujar separadores entre programaciones
        private void DgvReporte_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (dtReporte == null || e.RowIndex >= dtReporte.Rows.Count) return;

            // Verificar si es el inicio de una nueva programación
            if (e.RowIndex > 0)
            {
                string programacionActual = dtReporte.Rows[e.RowIndex]["Folio Programación"].ToString();
                string programacionAnterior = dtReporte.Rows[e.RowIndex - 1]["Folio Programación"].ToString();

                if (programacionActual != programacionAnterior && !string.IsNullOrEmpty(programacionActual))
                {
                    // Dibujar línea separadora (solo en pantalla, no en exportación)
                    using (Pen pen = new Pen(Color.DarkBlue, 2))
                    {
                        int y = e.RowBounds.Top - 1;
                        e.Graphics.DrawLine(pen,
                            e.RowBounds.Left, y,
                            e.RowBounds.Right, y);
                    }
                }
            }
        }

        private void ChkTodasProgramaciones_CheckedChanged(object sender, EventArgs e)
        {
            if (!doctoIdFiltro.HasValue) // Solo habilitar si no hay filtro específico
            {
                cmbProgramacion.Enabled = !chkTodasProgramaciones.Checked;
            }

            toolStripStatusLabel1.Text = chkTodasProgramaciones.Checked ?
                "Modo: Todas las programaciones" : "Modo: Programación específica";
        }

        private void CargarProgramaciones()
        {
            try
            {
                toolStripStatusLabel1.Text = "Cargando programaciones...";
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Style = ProgressBarStyle.Marquee;

                if (conn_sql.ConectarSQL())
                {
                    string query = @"
                        SELECT DOCTO_PR_ID, 
                               FOLIO + ' - $' + FORMAT(ISNULL(IMPORTE_AUTORIZADO, 0), 'N2') + ' (' + 
                               FORMAT(FECHA_PAGO, 'dd/MM/yyyy') + ')' AS Descripcion
                        FROM P_DOCTOS_PR 
                        WHERE ESTATUS_PROC = 'L' 
                        ORDER BY FECHA_PAGO DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn_sql.SC);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbProgramacion.DisplayMember = "Descripcion";
                    cmbProgramacion.ValueMember = "DOCTO_PR_ID";
                    cmbProgramacion.DataSource = dt;

                    // Si hay filtro específico, seleccionarlo
                    if (doctoIdFiltro.HasValue)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(dt.Rows[i]["DOCTO_PR_ID"]) == doctoIdFiltro.Value)
                            {
                                cmbProgramacion.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    toolStripStatusLabel1.Text = $"Se cargaron {dt.Rows.Count} programaciones disponibles";

                    conn_sql.Desconectar();
                }
                else
                {
                    throw new Exception("No se pudo conectar a la base de datos");
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error al cargar programaciones";
                MessageBox.Show($"Error al cargar programaciones: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                toolStripProgressBar1.Visible = false;
            }
        }

        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "Generando reporte...";
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
                btnGenerar.Enabled = false;

                GenerarReporte();
                ConfigurarDataGridView();
                CalcularTotales();
                btnExportar.Enabled = dtReporte.Rows.Count > 0;

                toolStripStatusLabel1.Text = $"Reporte generado: {dtReporte.Rows.Count} registros";
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error al generar reporte";
                MessageBox.Show($"Error al generar reporte: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                toolStripProgressBar1.Visible = false;
                btnGenerar.Enabled = true;
            }
        }

        private void GenerarReporte()
        {
            if (conn_sql.ConectarSQL())
            {
                try
                {
                    string whereClause;

                    // Determinar filtro basado en parámetro o selección
                    if (doctoIdFiltro.HasValue)
                    {
                        whereClause = $"WHERE pr.ESTATUS_PROC = 'L' AND pr.DOCTO_PR_ID = {doctoIdFiltro.Value}";
                    }
                    else
                    {
                        whereClause = chkTodasProgramaciones.Checked ?
                            "WHERE pr.ESTATUS_PROC = 'L'" :
                            $"WHERE pr.ESTATUS_PROC = 'L' AND pr.DOCTO_PR_ID = {cmbProgramacion.SelectedValue}";
                    }

                    string query = $@"
                        SELECT 
                            -- Información de Programación (solo una vez por programación)
                            CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY pr.DOCTO_PR_ID ORDER BY det.DOCTO_PR_DET_ID, req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN pr.FOLIO 
                                ELSE '' 
                            END as 'Folio Programación',

                            CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY pr.DOCTO_PR_ID ORDER BY det.DOCTO_PR_DET_ID, req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN det.EMPRESA 
                                ELSE '' 
                            END as 'Empresa',

							CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY pr.DOCTO_PR_ID ORDER BY det.DOCTO_PR_DET_ID, req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN PPM.Material 
                                ELSE '' 
                            END as 'Concepto',

							/* CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY pr.DOCTO_PR_ID ORDER BY det.DOCTO_PR_DET_ID, req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN PPTO.Depto_co_id 
                                ELSE '' 
                            END as 'DEPTO_CO_ID', */

							CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY pr.DOCTO_PR_ID ORDER BY det.DOCTO_PR_DET_ID, req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN PPTO.Nombre_Presupuesto 
                                ELSE '' 
                            END as 'Proyecto',

							CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY pr.DOCTO_PR_ID ORDER BY det.DOCTO_PR_DET_ID, req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN
									CASE
										WHEN SUBSTRING(PPM.Cuenta, 1, 1) = '6' THEN 'Gasto'
										ELSE 'Costo'
									END
                                ELSE '' 
                            END as 'Tipo gasto',

							CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY pr.DOCTO_PR_ID ORDER BY det.DOCTO_PR_DET_ID, req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN
									CASE
										WHEN SUBSTRING(PPM.Cuenta, 1, 1) = '6' THEN 'Corporativo'
										ELSE 'Cliente'
									END
                                ELSE '' 
                            END as 'Categoria',
                            
                            CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY pr.DOCTO_PR_ID ORDER BY det.DOCTO_PR_DET_ID, req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN FORMAT(pr.FECHA_PAGO, 'dd/MM/yyyy')
                                ELSE '' 
                            END as 'Fecha Pago',
                            
                            CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY pr.DOCTO_PR_ID ORDER BY det.DOCTO_PR_DET_ID, req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN FORMAT(ISNULL(pr.IMPORTE_AUTORIZADO, 0), 'C2')
                                ELSE '' 
                            END as 'Importe Total',
                            
                            CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY pr.DOCTO_PR_ID ORDER BY det.DOCTO_PR_DET_ID, req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN ISNULL(pr.USUARIO_AUTORIZO, 'N/A')
                                ELSE '' 
                            END as 'Autorizado Por',
                            
                            -- Información de Pago (una vez por proveedor)
                            CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY det.DOCTO_PR_DET_ID ORDER BY req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN ISNULL(det.PROVEEDOR_NOMBRE, 'N/A')
                                ELSE '' 
                            END as 'Proveedor',

                            CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY det.DOCTO_PR_DET_ID ORDER BY req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN ISNULL(det.FOLIO_MICROSIP, 'N/A')
                                ELSE '' 
                            END as 'Factura',
                            
                            CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY det.DOCTO_PR_DET_ID ORDER BY req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN FORMAT(ISNULL(det.IMPORTE_AUTORIZADO, 0), 'C2')
                                ELSE '' 
                            END as 'Importe Pagando',
                            
                            -- Total Requisición (solo una vez por requisición, no por proveedor)
                            CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY req.Requisicion_id ORDER BY det.DOCTO_PR_DET_ID, reqdet.Requisicion_det_id) = 1 
                                THEN FORMAT((
                                    SELECT SUM(ISNULL(rd.cantidad_requerida, 0) * ISNULL(rd.Precio_Unitario, 0))
                                    FROM REQ_DET rd 
                                    WHERE rd.Requisicion_id = req.Requisicion_id AND rd.Estatus = 'A'
                                ), 'C2')
                                ELSE '' 
                            END as 'Total Requisición',
                            
                            CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY det.DOCTO_PR_DET_ID ORDER BY req.Requisicion_id, reqdet.Requisicion_det_id) = 1 
                                THEN ISNULL(det.FOLIO_CREDITO, 'N/A')
                                ELSE '' 
                            END as 'Folio Crédito',
                            
                            -- Información de Requisición - Folio siempre visible
                            ISNULL(CAST(req.Folio AS VARCHAR), 'N/A') as 'Folio Req.',
                            
                            CASE 
                                WHEN ROW_NUMBER() OVER (PARTITION BY req.Requisicion_id ORDER BY reqdet.Requisicion_det_id) = 1 
                                THEN ISNULL(req.Lugar_destino, 'N/A')
                                ELSE '' 
                            END as 'Destino',
                            
                            -- Detalle de Materiales (cada material individual)
                            ISNULL(reqdet.SKU, 'N/A') as 'SKU',
                            ISNULL(reqdet.Material, 'Sin descripción') as 'Descripción Material',
                            FORMAT(ISNULL(reqdet.cantidad_requerida, 0), 'N0') as 'Cantidad',
                            FORMAT(ISNULL(reqdet.Precio_Unitario, 0), 'C2') as 'Precio Unit.',
                            FORMAT(ISNULL(reqdet.cantidad_requerida, 0) * ISNULL(reqdet.Precio_Unitario, 0), 'C2') as 'Subtotal',
                            ISNULL(reqdet.Comentarios, '') as 'Observaciones',
                            
                            -- Campos para agrupación y ordenamiento
                            pr.DOCTO_PR_ID,
                            det.DOCTO_PR_DET_ID,
                            req.Requisicion_id,
                            reqdet.Requisicion_det_id,
                            ISNULL(pr.IMPORTE_AUTORIZADO, 0) as ImporteNumerico,
                            ISNULL(reqdet.cantidad_requerida, 0) * ISNULL(reqdet.Precio_Unitario, 0) as SubtotalNumerico
                            
                        FROM P_DOCTOS_PR pr
                        INNER JOIN P_DOCTOS_PR_DET det ON pr.DOCTO_PR_ID = det.DOCTO_PR_ID
                        INNER JOIN REQ_ENC req ON det.REQUISICION_ID = req.Requisicion_id
                        INNER JOIN REQ_DET reqdet ON req.Requisicion_id = reqdet.Requisicion_id 
                                                   AND reqdet.Estatus = 'A'

                        INNER JOIN PPTO_PROD_MATERIALES PPM ON(reqdet.ppto_prod_material_id = PPM.Ppto_Prod_Material_Id)
						INNER JOIN PRESUPUESTOS PPTO ON(PPM.Presupuesto_Id = PPTO.Presupuesto_ID)

                        {whereClause}
                        AND det.ESTATUS = 'L'
                        ORDER BY 
                            pr.FECHA_PAGO DESC,
                            pr.FOLIO,
                            det.DOCTO_PR_DET_ID,
                            req.Requisicion_id,
                            reqdet.Requisicion_det_id";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn_sql.SC);
                    dtReporte = new DataTable();
                    da.Fill(dtReporte);

                    dgvReporte.DataSource = dtReporte;
                }
                finally
                {
                    conn_sql.Desconectar();
                }
            }
            else
            {
                throw new Exception("No se pudo conectar a la base de datos");
            }
        }

        private void ConfigurarDataGridView()
        {
            if (dtReporte == null || dtReporte.Rows.Count == 0) return;

            // Ocultar columnas de ID y auxiliares
            var columnasOcultar = new[] {
                "DOCTO_PR_ID", "DOCTO_PR_DET_ID", "Requisicion_id",
                "Requisicion_det_id", "ImporteNumerico", "SubtotalNumerico"
            };

            foreach (string col in columnasOcultar)
            {
                if (dgvReporte.Columns[col] != null)
                    dgvReporte.Columns[col].Visible = false;
            }

            // Configurar anchos de columnas optimizados para scroll horizontal
            var configuracionColumnas = new[]
            {
                new { Nombre = "Folio Programación", Ancho = 110, Frozen = true },
                new { Nombre = "Empresa", Ancho = 150, Frozen = true },
                new { Nombre = "Concepto", Ancho = 250, Frozen = true },
                new { Nombre = "Proyecto", Ancho = 150, Frozen = true },
                new { Nombre = "Tipo gasto", Ancho = 90, Frozen = true },
                new { Nombre = "Categoria", Ancho = 90, Frozen = true },
                new { Nombre = "Fecha Pago", Ancho = 85, Frozen = true },
                new { Nombre = "Importe Total", Ancho = 100, Frozen = false },
                new { Nombre = "Autorizado Por", Ancho = 100, Frozen = false },
                new { Nombre = "Proveedor", Ancho = 250, Frozen = false },
                new { Nombre = "Factura", Ancho = 85, Frozen = false },
                new { Nombre = "Importe Pagando", Ancho = 100, Frozen = false },
                new { Nombre = "Total Requisición", Ancho = 110, Frozen = false },
                new { Nombre = "Folio Crédito", Ancho = 100, Frozen = false },
                new { Nombre = "Folio Req.", Ancho = 70, Frozen = false },
                new { Nombre = "Destino", Ancho = 120, Frozen = false },
                new { Nombre = "SKU", Ancho = 80, Frozen = false },
                new { Nombre = "Descripción Material", Ancho = 300, Frozen = false },
                new { Nombre = "Cantidad", Ancho = 80, Frozen = false },
                new { Nombre = "Precio Unit.", Ancho = 100, Frozen = false },
                new { Nombre = "Subtotal", Ancho = 100, Frozen = false },
                new { Nombre = "Observaciones", Ancho = 200, Frozen = false }
            };

            foreach (var config in configuracionColumnas)
            {
                if (dgvReporte.Columns[config.Nombre] != null)
                {
                    dgvReporte.Columns[config.Nombre].Width = config.Ancho;
                    dgvReporte.Columns[config.Nombre].Frozen = config.Frozen;

                    // Configurar alineación específica
                    if (config.Nombre.Contains("Importe") || config.Nombre.Contains("Precio") ||
                        config.Nombre.Contains("Subtotal") || config.Nombre == "Cantidad")
                    {
                        dgvReporte.Columns[config.Nombre].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvReporte.Columns[config.Nombre].DefaultCellStyle.Font = new Font("Consolas", 9, FontStyle.Regular);
                    }

                    if (config.Nombre.Contains("Fecha"))
                    {
                        dgvReporte.Columns[config.Nombre].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }

            // Aplicar estilos ejecutivos
            AplicarEstilosEjecutivos();
        }

        private void AplicarEstilosEjecutivos()
        {
            if (dtReporte == null || dtReporte.Rows.Count == 0) return;

            string programacionActual = "";
            Color[] coloresProgramacion = {
                ColorTranslator.FromHtml("#F8F9FA"), // Gris muy claro
                ColorTranslator.FromHtml("#E3F2FD")   // Azul muy claro
            };
            int indiceProgramacion = 0;

            for (int i = 0; i < dgvReporte.Rows.Count; i++)
            {
                DataGridViewRow row = dgvReporte.Rows[i];
                string folioProgramacion = row.Cells["Folio Programación"].Value?.ToString() ?? "";

                // Detectar nueva programación
                if (folioProgramacion != programacionActual && !string.IsNullOrEmpty(folioProgramacion))
                {
                    programacionActual = folioProgramacion;
                    indiceProgramacion = (indiceProgramacion + 1) % coloresProgramacion.Length;
                }

                // Aplicar color de fondo alternado por programación
                Color colorFondo = coloresProgramacion[indiceProgramacion];
                row.DefaultCellStyle.BackColor = colorFondo;

                // Estilos especiales para filas con información de programación
                if (!string.IsNullOrEmpty(folioProgramacion))
                {
                    row.Cells["Folio Programación"].Style.BackColor = ColorTranslator.FromHtml("#2E7D32");
                    row.Cells["Folio Programación"].Style.ForeColor = Color.White;
                    row.Cells["Folio Programación"].Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                    row.Cells["Fecha Pago"].Style.BackColor = ColorTranslator.FromHtml("#2E7D32");
                    row.Cells["Fecha Pago"].Style.ForeColor = Color.White;
                    row.Cells["Fecha Pago"].Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }

                // Resaltar importes totales
                var importeTotal = row.Cells["Importe Total"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(importeTotal) && importeTotal != "$0.00")
                {
                    row.Cells["Importe Total"].Style.BackColor = ColorTranslator.FromHtml("#4CAF50");
                    row.Cells["Importe Total"].Style.ForeColor = Color.White;
                    row.Cells["Importe Total"].Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }

                // Resaltar subtotales de materiales
                var subtotal = row.Cells["Subtotal"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(subtotal) && subtotal != "$0.00")
                {
                    row.Cells["Subtotal"].Style.BackColor = ColorTranslator.FromHtml("#FF9800");
                    row.Cells["Subtotal"].Style.ForeColor = Color.White;
                    row.Cells["Subtotal"].Style.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                }

                // Resaltar proveedores
                var proveedor = row.Cells["Proveedor"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(proveedor))
                {
                    row.Cells["Proveedor"].Style.BackColor = ColorTranslator.FromHtml("#1976D2");
                    row.Cells["Proveedor"].Style.ForeColor = Color.White;
                    row.Cells["Proveedor"].Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }

                // Resaltar importes pagando vs totales de requisición
                var importePagando = row.Cells["Importe Pagando"].Value?.ToString() ?? "";
                var totalRequisicion = row.Cells["Total Requisición"].Value?.ToString() ?? "";

                if (!string.IsNullOrEmpty(importePagando) && importePagando != "$0.00")
                {
                    row.Cells["Importe Pagando"].Style.BackColor = ColorTranslator.FromHtml("#2196F3"); // Azul
                    row.Cells["Importe Pagando"].Style.ForeColor = Color.White;
                    row.Cells["Importe Pagando"].Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }

                if (!string.IsNullOrEmpty(totalRequisicion) && totalRequisicion != "$0.00")
                {
                    // Comparar si es pago parcial
                    decimal pagando = 0, total = 0;
                    if (decimal.TryParse(importePagando.Replace("$", "").Replace(",", ""), out pagando) &&
                        decimal.TryParse(totalRequisicion.Replace("$", "").Replace(",", ""), out total))
                    {
                        if (pagando < total)
                        {
                            // Pago parcial - color naranja
                            row.Cells["Total Requisición"].Style.BackColor = ColorTranslator.FromHtml("#FF9800");
                            row.Cells["Total Requisición"].Style.ForeColor = Color.White;
                            row.Cells["Total Requisición"].Style.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                        }
                        else
                        {
                            // Pago completo - color verde claro
                            row.Cells["Total Requisición"].Style.BackColor = ColorTranslator.FromHtml("#4CAF50");
                            row.Cells["Total Requisición"].Style.ForeColor = Color.White;
                            row.Cells["Total Requisición"].Style.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                        }
                    }
                }
            }

            // Configuración general del grid para look ejecutivo
            dgvReporte.EnableHeadersVisualStyles = false;
            dgvReporte.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#37474F");
            dgvReporte.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReporte.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvReporte.ColumnHeadersHeight = 35;

            dgvReporte.RowTemplate.Height = 25;
            dgvReporte.GridColor = ColorTranslator.FromHtml("#E0E0E0");
            dgvReporte.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dgvReporte.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#E1F5FE");
            dgvReporte.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#0277BD");
        }

        private void CalcularTotales()
        {
            if (dtReporte == null || dtReporte.Rows.Count == 0)
            {
                lblTotalImporte.Text = "Total: $0.00";
                return;
            }

            decimal totalGeneral = 0;
            HashSet<int> programacionesContadas = new HashSet<int>();

            foreach (DataRow row in dtReporte.Rows)
            {
                if (row["DOCTO_PR_ID"] != DBNull.Value)
                {
                    int programacionId = Convert.ToInt32(row["DOCTO_PR_ID"]);
                    if (!programacionesContadas.Contains(programacionId))
                    {
                        if (row["ImporteNumerico"] != DBNull.Value &&
                            decimal.TryParse(row["ImporteNumerico"].ToString(), out decimal importe))
                        {
                            totalGeneral += importe;
                            programacionesContadas.Add(programacionId);
                        }
                    }
                }
            }

            lblTotalImporte.Text = $"Total General: {totalGeneral:C2}";
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            if (dtReporte == null || dtReporte.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar", "Información",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                toolStripStatusLabel1.Text = "Exportando a Excel...";
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
                btnExportar.Enabled = false;

                ExportarAExcel();

                toolStripStatusLabel1.Text = "Exportación completada exitosamente";
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error durante la exportación";
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                toolStripProgressBar1.Visible = false;
                btnExportar.Enabled = true;
            }
        }

        private void ExportarAExcel()
        {
            Application excelApp = new Application();
            Workbook workbook = null;
            Worksheet worksheet = null;

            try
            {
                excelApp.Visible = false;
                workbook = excelApp.Workbooks.Add();
                worksheet = workbook.ActiveSheet;

                // Encabezado ejecutivo
                worksheet.Cells[1, 1] = "REPORTE EJECUTIVO - PROGRAMACIÓN DE PAGOS";
                Range titleRange = worksheet.Range["A1", "O1"];
                titleRange.Merge();
                titleRange.Font.Bold = true;
                titleRange.Font.Size = 14;
                titleRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                titleRange.Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml("#37474F"));
                titleRange.Font.Color = ColorTranslator.ToOle(Color.White);
                titleRange.RowHeight = 30;

                // Información del reporte
                worksheet.Cells[2, 1] = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
                worksheet.Cells[2, 1].Font.Size = 10;
                worksheet.Cells[3, 1] = $"Usuario: {Environment.UserName}";
                worksheet.Cells[3, 1].Font.Size = 10;

                string filtroInfo = doctoIdFiltro.HasValue ?
                    $"Programación específica: {doctoIdFiltro.Value}" :
                    (chkTodasProgramaciones.Checked ? "Todas las programaciones" : $"Filtro: {cmbProgramacion.Text}");

                worksheet.Cells[4, 1] = filtroInfo;
                worksheet.Cells[4, 1].Font.Size = 10;
                worksheet.Cells[4, 1].Font.Bold = true;

                int filaInicio = 6;

                // Contar columnas visibles
                int columnasVisibles = 0;
                foreach (DataGridViewColumn col in dgvReporte.Columns)
                {
                    if (col.Visible) columnasVisibles++;
                }

                // Verificar que hay datos y columnas para exportar
                if (dtReporte.Rows.Count == 0 || columnasVisibles == 0)
                {
                    worksheet.Cells[filaInicio, 1] = "No hay datos para mostrar";
                    excelApp.Visible = true;
                    return;
                }

                // Headers con formato ejecutivo
                int colIndex = 1;
                foreach (DataGridViewColumn col in dgvReporte.Columns)
                {
                    if (col.Visible)
                    {
                        worksheet.Cells[filaInicio, colIndex] = col.HeaderText;
                        Range headerCell = worksheet.Cells[filaInicio, colIndex];
                        headerCell.Font.Bold = true;
                        headerCell.Font.Size = 11;
                        headerCell.Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml("#ECEFF1"));
                        headerCell.Borders.LineStyle = XlLineStyle.xlContinuous;
                        colIndex++;
                    }
                }

                // Datos con formato sin separadores visuales (para Excel limpio)
                for (int row = 0; row < dtReporte.Rows.Count; row++)
                {
                    colIndex = 1;
                    string folioProgramacion = dtReporte.Rows[row]["Folio Programación"].ToString();

                    foreach (DataGridViewColumn col in dgvReporte.Columns)
                    {
                        if (col.Visible)
                        {
                            Range cell = worksheet.Cells[filaInicio + row + 1, colIndex];

                            // Obtener valor de la celda
                            object valor = dtReporte.Rows[row][col.Name];
                            if (valor != null && valor != DBNull.Value)
                            {
                                cell.Value = valor.ToString();
                            }
                            else
                            {
                                cell.Value = "";
                            }

                            // Formato especial para celdas con información de programación
                            if (!string.IsNullOrEmpty(folioProgramacion) &&
                                (col.Name == "Folio Programación" ||
                                 col.Name == "Fecha Pago" ||
                                 col.Name == "Importe Total"))
                            {
                                cell.Font.Bold = true;
                                cell.Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml("#E8F5E8"));
                            }

                            colIndex++;
                        }
                    }
                }

                // Auto ajustar columnas
                worksheet.Columns.AutoFit();

                // Congelar paneles en las primeras dos columnas (solo si hay columnas)
                if (columnasVisibles >= 3)
                {
                    worksheet.Range["C7"].Select();
                    excelApp.ActiveWindow.FreezePanes = true;
                }

                // Agregar filtros solo si hay datos y más de una fila
                if (dtReporte.Rows.Count > 0 && columnasVisibles > 0)
                {
                    try
                    {
                        string ultimaColumna = GetColumnLetter(columnasVisibles);
                        int ultimaFila = filaInicio + dtReporte.Rows.Count;

                        Range dataRange = worksheet.Range[$"A{filaInicio}:{ultimaColumna}{ultimaFila}"];
                        dataRange.AutoFilter();
                    }
                    catch (Exception ex)
                    {
                        // Si falla el AutoFilter, continuar sin él
                        System.Diagnostics.Debug.WriteLine($"Error aplicando AutoFilter: {ex.Message}");
                    }
                }

                // Resumen ejecutivo
                int filaResumen = filaInicio + dtReporte.Rows.Count + 3;
                worksheet.Cells[filaResumen, 1] = "RESUMEN EJECUTIVO";
                worksheet.Cells[filaResumen, 1].Font.Bold = true;
                worksheet.Cells[filaResumen, 1].Font.Size = 12;

                worksheet.Cells[filaResumen + 1, 1] = lblTotalImporte.Text;
                Range totalRange = worksheet.Range[$"A{filaResumen + 1}:C{filaResumen + 1}"];
                totalRange.Merge();
                totalRange.Font.Bold = true;
                totalRange.Font.Size = 11;
                totalRange.Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml("#C8E6C9"));
                totalRange.Borders.LineStyle = XlLineStyle.xlContinuous;

                excelApp.Visible = true;

                MessageBox.Show("Reporte ejecutivo exportado exitosamente", "Exportación Completada",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante la exportación: {ex.Message}\n\nDetalles: {ex.StackTrace}",
                               "Error de Exportación", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Intentar cerrar Excel si hubo error
                try
                {
                    if (excelApp != null)
                    {
                        excelApp.Quit();
                    }
                }
                catch { }
            }
            finally
            {
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        private string GetColumnLetter(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = string.Empty;

            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }

        // Método para liberar recursos personalizados
        private void LiberarRecursos()
        {
            // Liberar conexión SQL si existe
            if (conn_sql != null)
            {
                try
                {
                    conn_sql.Desconectar();
                }
                catch { }
                conn_sql = null;
            }

            // Liberar DataTable
            if (dtReporte != null)
            {
                dtReporte.Dispose();
                dtReporte = null;
            }
        }

        // Método para manejar el cierre del formulario
        private void F_REP_PROGRAMACION_FormClosing(object sender, FormClosingEventArgs e)
        {
            LiberarRecursos();
        }
    }
}