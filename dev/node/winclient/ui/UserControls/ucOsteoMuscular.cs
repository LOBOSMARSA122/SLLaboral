using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;

namespace Sigesoft.Node.WinClient.UI.UserControls
{
    public partial class UcOsteoMuscular : UserControl
    {

        private bool _isChangueValueControl;
        List<ServiceComponentFieldValuesList> _listOfAtencionAdulto1 = new List<ServiceComponentFieldValuesList>();
        private ServiceComponentFieldValuesList _userControlValores;

        #region "------------- Public Events -------------"
        /// <summary>
        /// Se desencadena cada vez que se cambia un valor del examen de Audiometria.
        /// </summary>
        public event EventHandler<AudiometriaAfterValueChangeEventArgs> AfterValueChange;
        protected void OnAfterValueChange(AudiometriaAfterValueChangeEventArgs e)
        {
            if (AfterValueChange != null)
                AfterValueChange(this, e);
        }
        #endregion

        #region "--------------- Properties --------------------"
        public string PersonId { get; set; }
        public string ServiceId { get; set; }

        public List<ServiceComponentFieldValuesList> DataSource
        {
            get
            {
                return _listOfAtencionAdulto1;
            }
            set
            {
                if (value != _listOfAtencionAdulto1)
                {
                    ClearValueControl();
                    _listOfAtencionAdulto1 = value;
                    SearchControlAndFill(value);
                }
            }
        }

        public void ClearValueControl()
        {
            _isChangueValueControl = false;
        }

        public bool IsChangeValueControl { get { return _isChangueValueControl; } }
        #endregion

        public UcOsteoMuscular()
        {
            InitializeComponent();
        }

        private void SearchControlAndFill(List<ServiceComponentFieldValuesList> dataSource)
        {
            if (dataSource == null || dataSource.Count == 0) return;
            // Ordenar Lista Datasource
            var dataSourceOrdenado = dataSource.OrderBy(p => p.v_ComponentFieldId).ToList();

            // recorrer la lista que viene de la BD
            foreach (var item in dataSourceOrdenado)
            {
                var matchedFields = Controls.Find(item.v_ComponentFieldId, true);

                if (matchedFields.Length > 0)
                {
                    var field = matchedFields[0];

                    if (field is TextBox)
                    {
                        if (field.Name == item.v_ComponentFieldId)
                        {
                            ((TextBox)field).Text = item.v_Value1;
                        }
                    }

                    else if (field is ComboBox)
                    {
                        if (field.Name == item.v_ComponentFieldId)
                        {
                            ((ComboBox)field).SelectedValue = item.v_Value1;
                        }
                    }
                }
            }
        }

        private void ucOsteoMuscular_Load(object sender, EventArgs e)
        {
            txtAnamnesis.Name = "N009-OTM00000001";//System.Windows.Forms.TextBox();
            cboCondiAmbEspacio.Name = "N009-OTM00000002";//System.Windows.Forms.ComboBox();
            cboCondiAmbTemperatura.Name = "N009-OTM00000003";//System.Windows.Forms.ComboBox();
            cboCondiAmbVibraciones.Name = "N009-OTM00000004";//System.Windows.Forms.ComboBox();
            cboCondiAmbSueloInestable.Name = "N009-OTM00000005";//System.Windows.Forms.ComboBox();
            cboCondiAmbDesniveles.Name = "N009-OTM00000006";//System.Windows.Forms.ComboBox();
            cboCondiAmbAltura.Name = "N009-OTM00000007";//System.Windows.Forms.ComboBox();
            cboCondiAmbPostura.Name = "N009-OTM00000008";//System.Windows.Forms.ComboBox();
            cboCondiAmbSuelo.Name = "N009-OTM00000009";//System.Windows.Forms.ComboBox();
            cboExigRitmo.Name = "N009-OTM00000010";//System.Windows.Forms.ComboBox();
            cboExigDistancias.Name = "N009-OTM00000011";//System.Windows.Forms.ComboBox();
            cboExigPeriodo.Name = "N009-OTM00000012";//System.Windows.Forms.ComboBox();
            cboExigEsfuerzo.Name = "N009-OTM00000013";//System.Windows.Forms.ComboBox();
            cboEsfFisAlzar.Name = "N009-OTM00000014";//System.Windows.Forms.ComboBox();
            cboEsfFisExiste.Name = "N009-OTM00000015";//System.Windows.Forms.ComboBox();
            cboEsfFisCuerpo.Name = "N009-OTM00000016";//System.Windows.Forms.ComboBox();
            cboEsfFisExige.Name = "N009-OTM00000017";//System.Windows.Forms.ComboBox();
            cboCarCargaPeso.Name = "N009-OTM00000018";//System.Windows.Forms.ComboBox();
            cboCarCargaManipulacion.Name = "N009-OTM00000019";//System.Windows.Forms.ComboBox();
            cboCarCargaEquilibrio.Name = "N009-OTM00000020";//System.Windows.Forms.ComboBox();
            cboCarCargaVolumen.Name = "N009-OTM00000021";//System.Windows.Forms.ComboBox();
            cboExpToxCadmio.Name = "N009-OTM00000022";//System.Windows.Forms.ComboBox();
            cboExpToxMercurio.Name = "N009-OTM00000023";//System.Windows.Forms.ComboBox();
            cboExpToxMagneso.Name = "N009-OTM00000024";//System.Windows.Forms.ComboBox();
            txt2Exposicion.Name = "N009-OTM00000025";//System.Windows.Forms.TextBox();
            txt2DiasSemana.Name = "N009-OTM00000026";//System.Windows.Forms.TextBox();
            txt2HorasSentado.Name = "N009-OTM00000027";//System.Windows.Forms.TextBox();
            txt2Repetitivo.Name = "N009-OTM00000028";//System.Windows.Forms.TextBox();
            txt2Anios.Name = "N009-OTM00000029";//System.Windows.Forms.TextBox();
            txt2Horas.Name = "N009-OTM00000030";//System.Windows.Forms.TextBox();
            cbo2RiesgoLevanta.Name = "N009-OTM00000031";//System.Windows.Forms.ComboBox();
            cbo2RiesgoDesplaza.Name = "N009-OTM00000032";//System.Windows.Forms.ComboBox();
            cbo2RiesgoSentado.Name = "N009-OTM00000033";//System.Windows.Forms.ComboBox();
            cbo2RiesgoPie.Name = "N009-OTM00000034";//System.Windows.Forms.ComboBox();
            cbo2RiesgoTracciona.Name = "N009-OTM00000035";//System.Windows.Forms.ComboBox();
            cbo2RiesgoEmpuja.Name = "N009-OTM00000036";//System.Windows.Forms.ComboBox();
            cbo2RiesgoColoca.Name = "N009-OTM00000037";//System.Windows.Forms.ComboBox();
            cboEjeLordosisCervical.Name = "N009-OTM00000038";//System.Windows.Forms.ComboBox();
            cboEjeCifosisDorsal.Name = "N009-OTM00000039";//System.Windows.Forms.ComboBox();
            cboEjeLordosisLumbar.Name = "N009-OTM00000040";//System.Windows.Forms.ComboBox();
            cboRotacionExt.Name = "N009-OTM00000041";//System.Windows.Forms.ComboBox();
            cboRotacionInt.Name = "N009-OTM00000042";//System.Windows.Forms.ComboBox();
            cboInversion.Name = "N009-OTM00000043";//System.Windows.Forms.ComboBox();
            cboPlantiflexion.Name = "N009-OTM00000044";//System.Windows.Forms.ComboBox();
            cboEversion.Name = "N009-OTM00000045";//System.Windows.Forms.ComboBox();
            cboDorsoflexion.Name = "N009-OTM00000046";//System.Windows.Forms.ComboBox();
            cboMmiiFlexion.Name = "N009-OTM00000047";//System.Windows.Forms.ComboBox();
            cboMmiiExtension.Name = "N009-OTM00000048";//System.Windows.Forms.ComboBox();
            cboMmiiContraResistencia.Name = "N009-OTM00000049";//System.Windows.Forms.ComboBox();
            cboCircunduccion.Name = "N009-OTM00000050";//System.Windows.Forms.ComboBox();
            cboDesviacionRadial.Name = "N009-OTM00000051";//System.Windows.Forms.ComboBox();
            cboExtension.Name = "N009-OTM00000052";//System.Windows.Forms.ComboBox();
            cboDesviacionCubital.Name = "N009-OTM00000053";//System.Windows.Forms.ComboBox();
            cboFlexion.Name = "N009-OTM00000054";//System.Windows.Forms.ComboBox();
            cboGenuvaro.Name = "N009-OTM00000055";//System.Windows.Forms.ComboBox();
            cboGenuvalgo.Name = "N009-OTM00000056";//System.Windows.Forms.ComboBox();
            cboPieCavo.Name = "N009-OTM00000057";//System.Windows.Forms.ComboBox();
            cboPiePlano.Name = "N009-OTM00000058";//System.Windows.Forms.ComboBox();
            cboEvaPropicepcion.Name = "N009-OTM00000059";//System.Windows.Forms.ComboBox();
            cboPruebaMancuerda.Name = "N009-OTM00000060";//System.Windows.Forms.ComboBox();
            cboRotulianoIzq.Name = "N009-OTM00000061";//System.Windows.Forms.ComboBox();
            cboRotulianoDer.Name = "N009-OTM00000062";//System.Windows.Forms.ComboBox();
            cboLasegueIzq.Name = "N009-OTM00000063";//System.Windows.Forms.ComboBox();
            cboSchoverIzq.Name = "N009-OTM00000064";//System.Windows.Forms.ComboBox();
            cboSchoverDer.Name = "N009-OTM00000065";//System.Windows.Forms.ComboBox();
            cboLasegueDer.Name = "N009-OTM00000066";//System.Windows.Forms.ComboBox();
            cboPhalenIzq.Name = "N009-OTM00000067";//System.Windows.Forms.ComboBox();
            cboTinelIzq.Name = "N009-OTM00000068";//System.Windows.Forms.ComboBox();
            cboTinelDer.Name = "N009-OTM00000069";//System.Windows.Forms.ComboBox();
            cboPhalenDer.Name = "N009-OTM00000070";//System.Windows.Forms.ComboBox();
            cboPalpacionDolor.Name = "N009-OTM00000071";//System.Windows.Forms.ComboBox();
            cboPalpacionContractura.Name = "N009-OTM00000072";//System.Windows.Forms.ComboBox();
            cboPalpacionApofisis.Name = "N009-OTM00000073";//System.Windows.Forms.ComboBox();
            txtPalpacionDolor.Name = "N009-OTM00000074";//System.Windows.Forms.TextBox();
            txtPalpacionContractura.Name = "N009-OTM00000075";//System.Windows.Forms.TextBox();
            txtPalpacionApofisis.Name = "N009-OTM00000076";//System.Windows.Forms.TextBox();
            cboLumbatExtension.Name = "N009-OTM00000077";//System.Windows.Forms.ComboBox();
            cboLumbatFlexion.Name = "N009-OTM00000078";//System.Windows.Forms.ComboBox();
            cboCervicalExtension.Name = "N009-OTM00000079";//System.Windows.Forms.ComboBox();
            cboCervicalFlexion.Name = "N009-OTM00000080";//System.Windows.Forms.ComboBox();
            cboLumbatLateIzquierda.Name = "N009-OTM00000081";//System.Windows.Forms.ComboBox();
            cboLumbatLateDerecha.Name = "N009-OTM00000082";//System.Windows.Forms.ComboBox();
            cboLumbatRotacionIzquierda.Name = "N009-OTM00000083";//System.Windows.Forms.ComboBox();
            cboLumbatRotacionDerecha.Name = "N009-OTM00000084";//System.Windows.Forms.ComboBox();
            cboCervicalLateIzquierda.Name = "N009-OTM00000085";//System.Windows.Forms.ComboBox();
            cboCervicalLateDerecha.Name = "N009-OTM00000086";//System.Windows.Forms.ComboBox();
            cboCervicalRotaIzquierda.Name = "N009-OTM00000087";//System.Windows.Forms.ComboBox();
            cboCervicalRotaDerecha.Name = "N009-OTM00000088";//System.Windows.Forms.ComboBox();
            cboLumbatIrradiacion.Name = "N009-OTM00000089";//System.Windows.Forms.ComboBox();
            cboCervicalIrradiacion.Name = "N009-OTM00000090";//System.Windows.Forms.ComboBox();
            cboAsimetriaEscoliosis.Name = "N009-OTM00000091";//System.Windows.Forms.ComboBox();
            cboAsimetriaHombros.Name = "N009-OTM00000092";//System.Windows.Forms.ComboBox();
            cboAsimetriaLumbar.Name = "N009-OTM00000093";//System.Windows.Forms.ComboBox();
            cboAsimetriaCaderas.Name = "N009-OTM00000094";//System.Windows.Forms.ComboBox();
            cboAsimetriaHipercifocis.Name = "N009-OTM00000095";//System.Windows.Forms.ComboBox();
            cboAsimetriaRodillas.Name = "N009-OTM00000096";//System.Windows.Forms.ComboBox();
            cboEquilibrioLateralIzquierdo.Name = "N009-OTM00000097";//System.Windows.Forms.ComboBox();
            cboEquilibrioLateralDerecho.Name = "N009-OTM00000098";//System.Windows.Forms.ComboBox();
            cboEquilibrioPosterior.Name = "N009-OTM00000099";//System.Windows.Forms.ComboBox();
            cboEquilibrioAnterior.Name = "N009-OTM00000100";//System.Windows.Forms.ComboBox();
            cboMarchaClaudicacion.Name = "N009-OTM00000101";//System.Windows.Forms.ComboBox();
            cboTobilloIzqRotInt.Name = "N009-OTM00000102";//System.Windows.Forms.ComboBox();
            cboRodillaIzqRotInt.Name = "N009-OTM00000103";//System.Windows.Forms.ComboBox();
            cboCaderaIzqRotInt.Name = "N009-OTM00000104";//System.Windows.Forms.ComboBox();
            cboMunecaIzqRotInt.Name = "N009-OTM00000105";//System.Windows.Forms.ComboBox();
            cboCodoIzqRotInt.Name = "N009-OTM00000106";//System.Windows.Forms.ComboBox();
            cboHombroIzqRotInt.Name = "N009-OTM00000107";//System.Windows.Forms.ComboBox();
            cboTobilloIzqExtension.Name = "N009-OTM00000108";//System.Windows.Forms.ComboBox();
            cboRodillaIzqExtension.Name = "N009-OTM00000109";//System.Windows.Forms.ComboBox();
            cboCaderaIzqExtension.Name = "N009-OTM00000110";//System.Windows.Forms.ComboBox();
            cboMunecaIzqExtension.Name = "N009-OTM00000111";//System.Windows.Forms.ComboBox();
            cboCodoIzqExtension.Name = "N009-OTM00000112";//System.Windows.Forms.ComboBox();
            cboTobilloIzqFlexion.Name = "N009-OTM00000113";//System.Windows.Forms.ComboBox();
            cboRodillaIzqFlexion.Name = "N009-OTM00000114";//System.Windows.Forms.ComboBox();
            cboCaderaIzqFlexion.Name = "N009-OTM00000115";//System.Windows.Forms.ComboBox();
            cboMunecaIzqFlexion.Name = "N009-OTM00000116";//System.Windows.Forms.ComboBox();
            cboHombroIzqExtension.Name = "N009-OTM00000117";//System.Windows.Forms.ComboBox();
            cboCodoIzqFlexion.Name = "N009-OTM00000118";//System.Windows.Forms.ComboBox();
            cboTobilloDerRotInt.Name = "N009-OTM00000119";//System.Windows.Forms.ComboBox();
            cboRodillaDerRotInt.Name = "N009-OTM00000120";//System.Windows.Forms.ComboBox();
            cboCaderaDerRotInt.Name = "N009-OTM00000121";//System.Windows.Forms.ComboBox();
            cboMunecaDerRotInt.Name = "N009-OTM00000122";//System.Windows.Forms.ComboBox();
            cboHombroIzqFlexion.Name = "N009-OTM00000123";//System.Windows.Forms.ComboBox();
            cboCodoDerRotInt.Name = "N009-OTM00000124";//System.Windows.Forms.ComboBox();
            cboTobilloDerExtension.Name = "N009-OTM00000125";//System.Windows.Forms.ComboBox();
            cboRodillaDerExtension.Name = "N009-OTM00000126";//System.Windows.Forms.ComboBox();
            cboCaderaDerExtension.Name = "N009-OTM00000127";//System.Windows.Forms.ComboBox();
            cboMunecaDerExtension.Name = "N009-OTM00000128";//System.Windows.Forms.ComboBox();
            cboHombroDerRotInt.Name = "N009-OTM00000129";//System.Windows.Forms.ComboBox();
            cboCodoDerExtension.Name = "N009-OTM00000130";//System.Windows.Forms.ComboBox();
            cboTobilloDerFlexion.Name = "N009-OTM00000131";//System.Windows.Forms.ComboBox();
            cboRodillaDerFlexion.Name = "N009-OTM00000132";//System.Windows.Forms.ComboBox();
            cboCaderaDerFlexion.Name = "N009-OTM00000133";//System.Windows.Forms.ComboBox();
            cboMunecaDerFlexion.Name = "N009-OTM00000134";//System.Windows.Forms.ComboBox();
            cboHombroDerExtension.Name = "N009-OTM00000135";//System.Windows.Forms.ComboBox();
            cboCodoDerFlexion.Name = "N009-OTM00000136";//System.Windows.Forms.ComboBox();
            cboHombroDerFlexion.Name = "N009-OTM00000137";//System.Windows.Forms.ComboBox();
            cboTobilloIzqTono.Name = "N009-OTM00000138";//System.Windows.Forms.ComboBox();
            cboRodillaIzqTono.Name = "N009-OTM00000139";//System.Windows.Forms.ComboBox();
            cboCaderaIzqTono.Name = "N009-OTM00000140";//System.Windows.Forms.ComboBox();
            cboTobilloIzqFuerza.Name = "N009-OTM00000141";//System.Windows.Forms.ComboBox();
            cboMunecaIzqTono.Name = "N009-OTM00000142";//System.Windows.Forms.ComboBox();
            cboRodillaIzqFuerza.Name = "N009-OTM00000143";//System.Windows.Forms.ComboBox();
            cboCaderaIzqFuerza.Name = "N009-OTM00000144";//System.Windows.Forms.ComboBox();
            cboCodoIzqTono.Name = "N009-OTM00000145";//System.Windows.Forms.ComboBox();
            cboTobilloIzqAbduccion.Name = "N009-OTM00000146";//System.Windows.Forms.ComboBox();
            cboMunecaIzqFuerza.Name = "N009-OTM00000147";//System.Windows.Forms.ComboBox();
            cboRodillaIzqAbduccion.Name = "N009-OTM00000148";//System.Windows.Forms.ComboBox();
            cboCaderaIzqAbduccion.Name = "N009-OTM00000149";//System.Windows.Forms.ComboBox();
            cboCodoIzqFuerza.Name = "N009-OTM00000150";//System.Windows.Forms.ComboBox();
            cboTobilloIzqAduccion.Name = "N009-OTM00000151";//System.Windows.Forms.ComboBox();
            cboMunecaIzqAbduccion.Name = "N009-OTM00000152";//System.Windows.Forms.ComboBox();
            cboRodillaIzqAduccion.Name = "N009-OTM00000153";//System.Windows.Forms.ComboBox();
            cboHombroIzqTono.Name = "N009-OTM00000154";//System.Windows.Forms.ComboBox();
            cboCaderaIzqAduccion.Name = "N009-OTM00000155";//System.Windows.Forms.ComboBox();
            cboCodoIzqAbduccion.Name = "N009-OTM00000156";//System.Windows.Forms.ComboBox();
            cboTobilloIzqRotExt.Name = "N009-OTM00000157";//System.Windows.Forms.ComboBox();
            cboMunecaIzqAduccion.Name = "N009-OTM00000158";//System.Windows.Forms.ComboBox();
            cboRodillaIzqRotExt.Name = "N009-OTM00000159";//System.Windows.Forms.ComboBox();
            cboHombroIzqFuerza.Name = "N009-OTM00000160";//System.Windows.Forms.ComboBox();
            cboCaderaIzqRotExt.Name = "N009-OTM00000161";//System.Windows.Forms.ComboBox();
            cboCodoIzqAduccion.Name = "N009-OTM00000162";//System.Windows.Forms.ComboBox();
            cboTobilloDerTono.Name = "N009-OTM00000163";//System.Windows.Forms.ComboBox();
            cboMunecaIzqRotExt.Name = "N009-OTM00000164";//System.Windows.Forms.ComboBox();
            cboRodillaDerTono.Name = "N009-OTM00000165";//System.Windows.Forms.ComboBox();
            cboHombroIzqAbduccion.Name = "N009-OTM00000166";//System.Windows.Forms.ComboBox();
            cboCaderaDerTono.Name = "N009-OTM00000167";//System.Windows.Forms.ComboBox();
            cboCodoIzqRotExt.Name = "N009-OTM00000168";//System.Windows.Forms.ComboBox();
            cboTobilloDerFuerza.Name = "N009-OTM00000169";//System.Windows.Forms.ComboBox();
            cboMunecaDerTono.Name = "N009-OTM00000170";//System.Windows.Forms.ComboBox();
            cboRodillaDerFuerza.Name = "N009-OTM00000171";//System.Windows.Forms.ComboBox();
            cboHombroIzqAduccion.Name = "N009-OTM00000172";//System.Windows.Forms.ComboBox();
            cboCaderaDerFuerza.Name = "N009-OTM00000173";//System.Windows.Forms.ComboBox();
            cboCodoDerTono.Name = "N009-OTM00000174";//System.Windows.Forms.ComboBox();
            cboTobilloDerAbduccion.Name = "N009-OTM00000175";//System.Windows.Forms.ComboBox();
            cboMunecaDerFuerza.Name = "N009-OTM00000176";//System.Windows.Forms.ComboBox();
            cboRodillaDerAbduccion.Name = "N009-OTM00000177";//System.Windows.Forms.ComboBox();
            cboHombroIzqRotExt.Name = "N009-OTM00000178";//System.Windows.Forms.ComboBox();
            cboCaderaDerAbduccion.Name = "N009-OTM00000179";//System.Windows.Forms.ComboBox();
            cboCodoDerFuerza.Name = "N009-OTM00000180";//System.Windows.Forms.ComboBox();
            cboTobilloDerAduccion.Name = "N009-OTM00000181";//System.Windows.Forms.ComboBox();
            cboMunecaDerAbduccion.Name = "N009-OTM00000182";//System.Windows.Forms.ComboBox();
            cboRodillaDerAduccion.Name = "N009-OTM00000183";//System.Windows.Forms.ComboBox();
            cboHombroDerTono.Name = "N009-OTM00000184";//System.Windows.Forms.ComboBox();
            cboCaderaDerAduccion.Name = "N009-OTM00000185";//System.Windows.Forms.ComboBox();
            cboCodoDerAbduccion.Name = "N009-OTM00000186";//System.Windows.Forms.ComboBox();
            cboTobilloDerRotExt.Name = "N009-OTM00000187";//System.Windows.Forms.ComboBox();
            cboMunecaDerAduccion.Name = "N009-OTM00000189";//System.Windows.Forms.ComboBox();
            cboRodillaDerRotExt.Name = "N009-OTM00000190";//System.Windows.Forms.ComboBox();
            cboHombroDerFuerza.Name = "N009-OTM00000191";//System.Windows.Forms.ComboBox();
            cboCaderaDerRotExt.Name = "N009-OTM00000192";//System.Windows.Forms.ComboBox();
            cboCodoDerAduccion.Name = "N009-OTM00000193";//System.Windows.Forms.ComboBox();
            cboTobilloIzqDolor.Name = "N009-OTM00000194";//System.Windows.Forms.ComboBox();
            cboMunecaDerRotExt.Name = "N009-OTM00000195";//System.Windows.Forms.ComboBox();
            cboRodillaIzqDolor.Name = "N009-OTM00000196";//System.Windows.Forms.ComboBox();
            cboHombroDerAbduccion.Name = "N009-OTM00000197";//System.Windows.Forms.ComboBox();
            cboCaderaIzqDolor.Name = "N009-OTM00000198";//System.Windows.Forms.ComboBox();
            cboCodoDerRotExt.Name = "N009-OTM00000199";//System.Windows.Forms.ComboBox();
            cboTobilloDerDolor.Name = "N009-OTM00000200";//System.Windows.Forms.ComboBox();
            cboMunecaIzqDolor.Name = "N009-OTM00000201";//System.Windows.Forms.ComboBox();
            cboRodillaDerDolor.Name = "N009-OTM00000202";//System.Windows.Forms.ComboBox();
            cboHombroDerAduccion.Name = "N009-OTM00000203";//System.Windows.Forms.ComboBox();
            cboCaderaDerDolor.Name = "N009-OTM00000204";//System.Windows.Forms.ComboBox();
            cboCodoIzqDolor.Name = "N009-OTM00000205";//System.Windows.Forms.ComboBox();
            cboMunecaDerDolor.Name = "N009-OTM00000206";//System.Windows.Forms.ComboBox();
            cboHombroDerRotExt.Name = "N009-OTM00000207";//System.Windows.Forms.ComboBox();
            cboCodoDerDolor.Name = "N009-OTM00000208";//System.Windows.Forms.ComboBox();
            cboHombroIzqDolor.Name = "N009-OTM00000209";//System.Windows.Forms.ComboBox();
            cboHombroDerDolor.Name = "N009-OTM00000210";//System.Windows.Forms.ComboBox();
            cboCodoIzqSupinacion.Name = "N009-OTM00000211";//System.Windows.Forms.ComboBox();
            cboCodoIzqPronacion.Name = "N009-OTM00000212";//System.Windows.Forms.ComboBox();
            cboCodoDerSupinacion.Name = "N009-OTM00000213";//System.Windows.Forms.ComboBox();
            cboCodoDerPronacion.Name = "N009-OTM00000214";//System.Windows.Forms.ComboBox();
            cboMunecaIzqRadial.Name = "N009-OTM00000215";//System.Windows.Forms.ComboBox();
            cboMunecaIzqCubital.Name = "N009-OTM00000216";//System.Windows.Forms.ComboBox();
            cboMunecaDerRadial.Name = "N009-OTM00000217";//System.Windows.Forms.ComboBox();
            cboMunecaDerCubital.Name = "N009-OTM00000218";//System.Windows.Forms.ComboBox();
            cboMunecaIzqInversion.Name = "N009-OTM00000219";//System.Windows.Forms.ComboBox();
            cboMunecaIzqEversion.Name = "N009-OTM00000220";//System.Windows.Forms.ComboBox();
            cboMunecaDerInversion.Name = "N009-OTM00000221";//System.Windows.Forms.ComboBox();
            cboMunecaDerEversion.Name = "N009-OTM00000222";//System.Windows.Forms.ComboBox();
            txtDescripcionHallazgos.Name = "N009-OTM00000223";//System.Windows.Forms.TextBox();
            cboConclusiones.Name = "N009-OTM00000224";//System.Windows.Forms.ComboBox();


            rbAbdomenExcelente.Name = "N009-OTM00000225";
            rbAbdomenPromedio.Name = "N009-OTM00000225";
            rbAbdomenRegular.Name = "N009-OTM00000226";
            rbAbdomenPobre.Name = "N009-OTM00000227";
            txtAbdomenPuntos.Name = "N009-OTM00000228";
            txtAbdomenObservaciones.Name = "N009-OTM00000229";

            rbCaderaExcelente.Name = "N009-OTM00000230";
            rbCaderaPromedio.Name = "N009-OTM00000231";
            rbCaderaRegular.Name = "N009-OTM00000232";
            rbCaderaPobre.Name = "N009-OTM00000233";
            txtCaderaPuntos.Name = "N009-OTM00000234";
            txtCaderaOnservaciones.Name = "N009-OTM00000235";

            rbMusloExcelente.Name = "N009-OTM00000236";
            rbMusloPromedio.Name = "N009-OTM00000237";
            rbMusloRegular.Name = "N009-OTM00000238";
            rbMusloPobre.Name = "N009-OTM00000239";
            txtMusloPuntos.Name = "N009-OTM00000240";
            txtMusloObservaciones.Name = "N009-OTM00000241";

            rbAbdomenLateralExcelente.Name = "N009-OTM00000242";
            rbAbdomenLateralPromedio.Name = "N009-OTM00000243";
            rbAbdomenLateralRegular.Name = "N009-OTM00000244";
            rbAbdomenLateralPobre.Name = "N009-OTM00000245";
            txtAbdomenLateralPuntos.Name = "N009-OTM00000246";
            txtAbdomenLateralObservaciones.Name = "N009-OTM00000247";

            rbAbduccion180Optimo.Name = "N009-OTM00000248";
            rbAbduccion180Limitado.Name = "N009-OTM00000249";
            rbAbduccion180MuyLimitado.Name = "N009-OTM00000250";
            txtAbduccion180Puntos.Name = "N009-OTM00000251";
            rbAbduccion180DolorSI.Name = "N009-OTM00000252";
            rbAbduccion180DolorNO.Name = "N009-OTM00000253";

            rbAbduccion60Optimo.Name = "N009-OTM00000254";
            rbAbduccion60Limitado.Name = "N009-OTM00000255";
            rbAbduccion60MuyLimitado.Name = "N009-OTM00000256";
            txtAbduccion60Puntos.Name = "N009-OTM00000257";
            rbAbduccion60DolorSI.Name = "N009-OTM00000258";
            rbAbduccion60DolorNO.Name = "N009-OTM00000259";

            rbRotacion090Optimo.Name = "N009-OTM00000260";
            rbRotacion090Limitado.Name = "N009-OTM00000261";
            rbRotacion090MuyLimitado.Name = "N009-OTM00000262";
            txtRotacion090Puntos.Name = "N009-OTM00000263";
            rbRotacion090DolorSI.Name = "N009-OTM00000264";
            rbRotacion090DolorNO.Name = "N009-OTM00000265";

            rbRotacionExtIntOptimo.Name = "N009-OTM00000266";
            rbRotacionExtIntLimitado.Name = "N009-OTM00000267";
            rbRotacionExtIntMuyLimitado.Name = "N009-OTM00000268";
            txtRotacionExtIntPuntos.Name = "N009-OTM00000269";
            rbRotacionExtIntDolorSI.Name = "N009-OTM00000270";
            rbRotacionExtIntDolorNO.Name = "N009-OTM00000271";
            
            txtTotalAptitudEspalda.Name = "N009-OTM00000272";

            txtTotalRangos.Name = "N009-OTM00000273";

            SearchControlAndSetEvents(this);
        }

        private void SearchControlAndSetEvents(Control ctrlContainer)
        {
            foreach (Control ctrl in ctrlContainer.Controls)
            {
                if (ctrl is TextBox)
                {
                    if (ctrl.Name.Contains("N009-OTM"))
                    {
                        ctrl.Leave += lbl_Leave;
                    }
                }
                if (ctrl is ComboBox)
                {
                    if (ctrl.Name.Contains("N009-CSN"))
                    {
                        var obj = (ComboBox)ctrl;
                        obj.SelectedValueChanged += ucd_TextChanged;
                    }
                }
                if (ctrl.HasChildren)
                    SearchControlAndSetEvents(ctrl);
            }
        }

        private void ucd_TextChanged(object sender, EventArgs e)
        {
            ComboBox senderCtrl = (ComboBox)sender;
            SaveValueControlForInterfacingEso(senderCtrl.Name, senderCtrl.SelectedValue.ToString());
            _isChangueValueControl = true;
        }

        private void lbl_Leave(object sender, EventArgs e)
        {
            TextBox senderCtrl = (TextBox)sender;
            SaveValueControlForInterfacingEso(senderCtrl.Name, senderCtrl.Text);
            _isChangueValueControl = true;
        }

        private void SaveValueControlForInterfacingEso(string name, string value)
        {
            #region Capturar Valor del campo

            _listOfAtencionAdulto1.RemoveAll(p => p.v_ComponentFieldId == name);

            _userControlValores = new ServiceComponentFieldValuesList();

            _userControlValores.v_ComponentFieldId = name;
            _userControlValores.v_Value1 = value;
            _userControlValores.v_ComponentId = Constants.OSTEO_MUSCULAR_UC;

            _listOfAtencionAdulto1.Add(_userControlValores);

            DataSource = _listOfAtencionAdulto1;

            #endregion
        }

    }
}
