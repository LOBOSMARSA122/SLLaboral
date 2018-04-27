using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;

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
              

                SaveValueControlForInterfacingESO(   "N009-OTM00000031",  cbo2RiesgoLevanta.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000002",  cboCondiAmbEspacio.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000003",  cboCondiAmbTemperatura.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000004",  cboCondiAmbVibraciones.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000005",  cboCondiAmbSueloInestable.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000006",  cboCondiAmbDesniveles.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000007",  cboCondiAmbAltura.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000008",  cboCondiAmbPostura.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000009",  cboCondiAmbSuelo.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000010",  cboExigRitmo.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000011",  cboExigDistancias.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000012",  cboExigPeriodo.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000013",  cboExigEsfuerzo.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000014",  cboEsfFisAlzar.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000015",  cboEsfFisExiste.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000016",  cboEsfFisCuerpo.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000017",  cboEsfFisExige.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000018",  cboCarCargaPeso.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000019",  cboCarCargaManipulacion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000020",  cboCarCargaEquilibrio.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000021",  cboCarCargaVolumen.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000022",  cboExpToxCadmio.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000023",  cboExpToxMercurio.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000024",  cboExpToxMagneso.SelectedValue.ToString());
               
                SaveValueControlForInterfacingESO(   "N009-OTM00000031",  cbo2RiesgoLevanta.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000032",  cbo2RiesgoDesplaza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000033",  cbo2RiesgoSentado.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000034",  cbo2RiesgoPie.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000035",  cbo2RiesgoTracciona.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000036",  cbo2RiesgoEmpuja.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000037",  cbo2RiesgoColoca.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000038",  cboEjeLordosisCervical.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000039",  cboEjeCifosisDorsal.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000040",  cboEjeLordosisLumbar.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000041",  cboRotacionExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000042",  cboRotacionInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000043",  cboInversion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000044",  cboPlantiflexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000045",  cboEversion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000046",  cboDorsoflexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000047",  cboMmiiFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000048",  cboMmiiExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000049",  cboMmiiContraResistencia.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000050",  cboCircunduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000051",  cboDesviacionRadial.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000052",  cboExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000053",  cboDesviacionCubital.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000054",  cboFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000055",  cboGenuvaro.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000056",  cboGenuvalgo.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000057",  cboPieCavo.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000058",  cboPiePlano.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000059",  cboEvaPropicepcion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000060",  cboPruebaMancuerda.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000061",  cboRotulianoIzq.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000062",  cboRotulianoDer.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000063",  cboLasegueIzq.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000064",  cboSchoverIzq.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000065",  cboSchoverDer.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000066",  cboLasegueDer.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000067",  cboPhalenIzq.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000068",  cboTinelIzq.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000069",  cboTinelDer.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000070",  cboPhalenDer.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000071",  cboPalpacionDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000072",  cboPalpacionContractura.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000073",  cboPalpacionApofisis.SelectedValue.ToString());
              
                SaveValueControlForInterfacingESO(   "N009-OTM00000077",  cboLumbatExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000078",  cboLumbatFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000079",  cboCervicalExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000080",  cboCervicalFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000081",  cboLumbatLateIzquierda.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000082",  cboLumbatLateDerecha.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000083",  cboLumbatRotacionIzquierda.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000084",  cboLumbatRotacionDerecha.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000085",  cboCervicalLateIzquierda.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000086",  cboCervicalLateDerecha.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000087",  cboCervicalRotaIzquierda.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000088",  cboCervicalRotaDerecha.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000089",  cboLumbatIrradiacion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000090",  cboCervicalIrradiacion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000091",  cboAsimetriaEscoliosis.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000092",  cboAsimetriaHombros.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000093",  cboAsimetriaLumbar.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000094",  cboAsimetriaCaderas.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000095",  cboAsimetriaHipercifocis.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000096",  cboAsimetriaRodillas.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000097",  cboEquilibrioLateralIzquierdo.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000098",  cboEquilibrioLateralDerecho.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000099",  cboEquilibrioPosterior.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000100",  cboEquilibrioAnterior.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000101",  cboMarchaClaudicacion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000102",  cboTobilloIzqRotInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000103",  cboRodillaIzqRotInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000104",  cboCaderaIzqRotInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000105",  cboMunecaIzqRotInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000106",  cboCodoIzqRotInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000107",  cboHombroIzqRotInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000108",  cboTobilloIzqExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000109",  cboRodillaIzqExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000110",  cboCaderaIzqExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000111",  cboMunecaIzqExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000112",  cboCodoIzqExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000113",  cboTobilloIzqFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000114",  cboRodillaIzqFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000115",  cboCaderaIzqFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000116",  cboMunecaIzqFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000117",  cboHombroIzqExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000118",  cboCodoIzqFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000119",  cboTobilloDerRotInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000120",  cboRodillaDerRotInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000121",  cboCaderaDerRotInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000122",  cboMunecaDerRotInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000123",  cboHombroIzqFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000124",  cboCodoDerRotInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000125",  cboTobilloDerExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000126",  cboRodillaDerExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000127",  cboCaderaDerExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000128",  cboMunecaDerExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000129",  cboHombroDerRotInt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000130",  cboCodoDerExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000131",  cboTobilloDerFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000132",  cboRodillaDerFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000133",  cboCaderaDerFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000134",  cboMunecaDerFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000135",  cboHombroDerExtension.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000136",  cboCodoDerFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000137",  cboHombroDerFlexion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000138",  cboTobilloIzqTono.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000139",  cboRodillaIzqTono.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000140",  cboCaderaIzqTono.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000141",  cboTobilloIzqFuerza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000142",  cboMunecaIzqTono.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000143",  cboRodillaIzqFuerza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000144",  cboCaderaIzqFuerza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000145",  cboCodoIzqTono.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000146",  cboTobilloIzqAbduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000147",  cboMunecaIzqFuerza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000148",  cboRodillaIzqAbduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000149",  cboCaderaIzqAbduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000150",  cboCodoIzqFuerza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000151",  cboTobilloIzqAduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000152",  cboMunecaIzqAbduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000153",  cboRodillaIzqAduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000154",  cboHombroIzqTono.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000155",  cboCaderaIzqAduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000156",  cboCodoIzqAbduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000157",  cboTobilloIzqRotExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000158",  cboMunecaIzqAduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000159",  cboRodillaIzqRotExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000160",  cboHombroIzqFuerza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000161",  cboCaderaIzqRotExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000162",  cboCodoIzqAduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000163",  cboTobilloDerTono.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000164",  cboMunecaIzqRotExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000165",  cboRodillaDerTono.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000166",  cboHombroIzqAbduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000167",  cboCaderaDerTono.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000168",  cboCodoIzqRotExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000169",  cboTobilloDerFuerza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000170",  cboMunecaDerTono.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000171",  cboRodillaDerFuerza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000172",  cboHombroIzqAduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000173",  cboCaderaDerFuerza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000174",  cboCodoDerTono.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000175",  cboTobilloDerAbduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000176",  cboMunecaDerFuerza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000177",  cboRodillaDerAbduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000178",  cboHombroIzqRotExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000179",  cboCaderaDerAbduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000180",  cboCodoDerFuerza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000181",  cboTobilloDerAduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000182",  cboMunecaDerAbduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000183",  cboRodillaDerAduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000184",  cboHombroDerTono.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000185",  cboCaderaDerAduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000186",  cboCodoDerAbduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000187",  cboTobilloDerRotExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000189",  cboMunecaDerAduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000190",  cboRodillaDerRotExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000191",  cboHombroDerFuerza.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000192",  cboCaderaDerRotExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000193",  cboCodoDerAduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000194",  cboTobilloIzqDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000195",  cboMunecaDerRotExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000196",  cboRodillaIzqDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000197",  cboHombroDerAbduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000198",  cboCaderaIzqDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000221",  cboCodoDerRotExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000200",  cboTobilloDerDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000201",  cboMunecaIzqDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000202",  cboRodillaDerDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000203",  cboHombroDerAduccion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000204",  cboCaderaDerDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000205",  cboCodoIzqDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000206",  cboMunecaDerDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000207",  cboHombroDerRotExt.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000208",  cboCodoDerDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000209",  cboHombroIzqDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000210",  cboHombroDerDolor.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000211",  cboCodoIzqSupinacion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000212",  cboCodoIzqPronacion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000213",  cboCodoDerSupinacion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000214",  cboCodoDerPronacion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000215",  cboMunecaIzqRadial.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000216",  cboMunecaIzqCubital.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000217",  cboMunecaDerRadial.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000218",  cboMunecaDerCubital.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000219",  cboMunecaIzqInversion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000220",  cboMunecaIzqEversion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000221",  cboMunecaDerInversion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000222",  cboMunecaDerEversion.SelectedValue.ToString());
                SaveValueControlForInterfacingESO(   "N009-OTM00000224", cboConclusiones.SelectedValue.ToString());


                //rbRotacionExtIntDolorSI.Name = "N009-OTM00000270";
                //rbRotacion090DolorSI.Name = "N009-OTM00000264";
                //rbAbduccion60DolorSI.Name = "N009-OTM00000258";
                //rbAbduccion180DolorSI.Name = "N009-OTM00000252";
                SaveValueControlForInterfacingESO("N009-OTM00000252", "2");
                SaveValueControlForInterfacingESO("N009-OTM00000258", "2");
                SaveValueControlForInterfacingESO("N009-OTM00000264", "2");
                SaveValueControlForInterfacingESO("N009-OTM00000270", "2");



                SaveValueControlForInterfacingESO("N009-OTM00000249", txtAbduccion180Puntos.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000257", txtAbduccion60Puntos.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000263", txtRotacion090Puntos.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000269", txtRotacionExtIntPuntos.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000272", txtTotalAptitudEspalda.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000273", txtTotalRangos.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000228", txtAbdomenPuntos.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000234", txtCaderaPuntos.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000240", txtMusloPuntos.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000246", txtAbdomenLateralPuntos.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000229", txtAbdomenObservaciones.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000235", txtCaderaOnservaciones.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000247", txtAbdomenLateralObservaciones.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000241", txtMusloObservaciones.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000001", txtAnamnesis.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000028", txt2Repetitivo.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000025", txt2Exposicion.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000026", txt2DiasSemana.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000027", txt2HorasSentado.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000029", txt2Anios.Text);
                SaveValueControlForInterfacingESO("N009-OTM00000030", txt2Horas.Text);


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
                    else if (field is RadioButton)
                    {
                        if (field.Name == item.v_ComponentFieldId)
                        {
                            if (item.v_ComponentFieldId == "N009-OTM00000274")
                            {
                                if (item.v_Value1.ToString() == "1")
                                {
                                    rbAbdomenExcelente.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "2")
                                {
                                    rbAbdomenPromedio.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "3")
                                {
                                    rbAbdomenRegular.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "4")
                                {
                                    rbAbdomenPobre.Checked = true;
                                }
                            }

                            if (item.v_ComponentFieldId == "N009-OTM00000230")
                            {
                                if (item.v_Value1.ToString() == "1")
                                {
                                    rbCaderaExcelente.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "2")
                                {
                                    rbCaderaPromedio.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "3")
                                {
                                    rbCaderaRegular.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "4")
                                {
                                    rbCaderaPobre.Checked = true;
                                }
                            }
                            if (item.v_ComponentFieldId == "N009-OTM00000236")
                            {
                                if (item.v_Value1.ToString() == "1")
                                {
                                    rbMusloExcelente.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "2")
                                {
                                    rbMusloPromedio.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "3")
                                {
                                    rbMusloRegular.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "4")
                                {
                                    rbMusloPobre.Checked = true;
                                }
                            }

                            if (item.v_ComponentFieldId == "N009-OTM00000242")
                            {
                                if (item.v_Value1.ToString() == "1")
                                {
                                    rbAbdomenLateralExcelente.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "2")
                                {
                                    rbAbdomenLateralPromedio.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "3")
                                {
                                    rbAbdomenLateralRegular.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "4")
                                {
                                    rbAbdomenLateralPobre.Checked = true;
                                }
                            }

                            if (item.v_ComponentFieldId == "N009-OTM00000248")
                            {
                                if (item.v_Value1.ToString() == "1")
                                {
                                    rbAbduccion180Optimo.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "2")
                                {
                                    rbAbduccion180Limitado.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "3")
                                {
                                    rbAbduccion180MuyLimitado.Checked = true;
                                }
                            }

                            if (item.v_ComponentFieldId == "N009-OTM00000254")
                            {
                                if (item.v_Value1.ToString() == "1")
                                {
                                    rbAbduccion60Optimo.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "2")
                                {
                                    rbAbduccion60Limitado.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "3")
                                {
                                    rbAbduccion60MuyLimitado.Checked = true;
                                }
                            }

                            if (item.v_ComponentFieldId == "N009-OTM00000252")
                            {
                                if (item.v_Value1.ToString() == "1")
                                {
                                    rbAbduccion180DolorSI.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "2")
                                {
                                    rbAbduccion180DolorNO.Checked = true;
                                }
                            }


                            if (item.v_ComponentFieldId == "N009-OTM00000260")
                            {
                                if (item.v_Value1.ToString() == "1")
                                {
                                    rbRotacion090Optimo.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "2")
                                {
                                    rbRotacion090Limitado.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "3")
                                {
                                    rbRotacion090MuyLimitado.Checked = true;
                                }
                            }

                            if (item.v_ComponentFieldId == "N009-OTM00000266")
                            {
                                if (item.v_Value1.ToString() == "1")
                                {
                                    rbRotacionExtIntOptimo.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "2")
                                {
                                    rbRotacionExtIntLimitado.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "3")
                                {
                                    rbRotacionExtIntMuyLimitado.Checked = true;
                                }
                            }

                          

                            if (item.v_ComponentFieldId == "N009-OTM00000258")
                            {
                                if (item.v_Value1.ToString() == "1")
                                {
                                    rbAbduccion60DolorSI.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "2")
                                {
                                    rbAbduccion60DolorNO.Checked = true;
                                }
                            }

                            if (item.v_ComponentFieldId == "N009-OTM00000264")
                            {
                                if (item.v_Value1.ToString() == "1")
                                {
                                    rbRotacion090DolorSI.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "2")
                                {
                                    rbRotacion090DolorNO.Checked = true;
                                }
                            }

                            if (item.v_ComponentFieldId == "N009-OTM00000270")
                            {
                                if (item.v_Value1.ToString() == "1")
                                {
                                    rbRotacionExtIntDolorSI.Checked = true;
                                }
                                else if (item.v_Value1.ToString() == "2")
                                {
                                    rbRotacionExtIntDolorNO.Checked = true;
                                }
                            }

                        }
                    }
                }
            }
        }

        private void ucOsteoMuscular_Load(object sender, EventArgs e)
        {
            txtAnamnesis.Name = "N009-OTM00000001"; 
            cboCondiAmbEspacio.Name = "N009-OTM00000002"; 
            cboCondiAmbTemperatura.Name = "N009-OTM00000003"; 
            cboCondiAmbVibraciones.Name = "N009-OTM00000004"; 
            cboCondiAmbSueloInestable.Name = "N009-OTM00000005"; 
            cboCondiAmbDesniveles.Name = "N009-OTM00000006"; 
            cboCondiAmbAltura.Name = "N009-OTM00000007"; 
            cboCondiAmbPostura.Name = "N009-OTM00000008"; 
            cboCondiAmbSuelo.Name = "N009-OTM00000009"; 
            cboExigRitmo.Name = "N009-OTM00000010"; 
            cboExigDistancias.Name = "N009-OTM00000011"; 
            cboExigPeriodo.Name = "N009-OTM00000012"; 
            cboExigEsfuerzo.Name = "N009-OTM00000013"; 
            cboEsfFisAlzar.Name = "N009-OTM00000014"; 
            cboEsfFisExiste.Name = "N009-OTM00000015"; 
            cboEsfFisCuerpo.Name = "N009-OTM00000016"; 
            cboEsfFisExige.Name = "N009-OTM00000017"; 
            cboCarCargaPeso.Name = "N009-OTM00000018"; 
            cboCarCargaManipulacion.Name = "N009-OTM00000019"; 
            cboCarCargaEquilibrio.Name = "N009-OTM00000020"; 
            cboCarCargaVolumen.Name = "N009-OTM00000021"; 
            cboExpToxCadmio.Name = "N009-OTM00000022"; 
            cboExpToxMercurio.Name = "N009-OTM00000023"; 
            cboExpToxMagneso.Name = "N009-OTM00000024"; 
            txt2Exposicion.Name = "N009-OTM00000025"; 
            txt2DiasSemana.Name = "N009-OTM00000026"; 
            txt2HorasSentado.Name = "N009-OTM00000027"; 
            txt2Repetitivo.Name = "N009-OTM00000028"; 
            txt2Anios.Name = "N009-OTM00000029"; 
            txt2Horas.Name = "N009-OTM00000030"; 
            cbo2RiesgoLevanta.Name = "N009-OTM00000031"; 
            cbo2RiesgoDesplaza.Name = "N009-OTM00000032"; 
            cbo2RiesgoSentado.Name = "N009-OTM00000033"; 
            cbo2RiesgoPie.Name = "N009-OTM00000034"; 
            cbo2RiesgoTracciona.Name = "N009-OTM00000035"; 
            cbo2RiesgoEmpuja.Name = "N009-OTM00000036"; 
            cbo2RiesgoColoca.Name = "N009-OTM00000037"; 
            cboEjeLordosisCervical.Name = "N009-OTM00000038"; 
            cboEjeCifosisDorsal.Name = "N009-OTM00000039"; 
            cboEjeLordosisLumbar.Name = "N009-OTM00000040"; 
            cboRotacionExt.Name = "N009-OTM00000041"; 
            cboRotacionInt.Name = "N009-OTM00000042"; 
            cboInversion.Name = "N009-OTM00000043"; 
            cboPlantiflexion.Name = "N009-OTM00000044"; 
            cboEversion.Name = "N009-OTM00000045"; 
            cboDorsoflexion.Name = "N009-OTM00000046"; 
            cboMmiiFlexion.Name = "N009-OTM00000047"; 
            cboMmiiExtension.Name = "N009-OTM00000048"; 
            cboMmiiContraResistencia.Name = "N009-OTM00000049"; 
            cboCircunduccion.Name = "N009-OTM00000050"; 
            cboDesviacionRadial.Name = "N009-OTM00000051"; 
            cboExtension.Name = "N009-OTM00000052"; 
            cboDesviacionCubital.Name = "N009-OTM00000053"; 
            cboFlexion.Name = "N009-OTM00000054"; 
            cboGenuvaro.Name = "N009-OTM00000055"; 
            cboGenuvalgo.Name = "N009-OTM00000056"; 
            cboPieCavo.Name = "N009-OTM00000057"; 
            cboPiePlano.Name = "N009-OTM00000058"; 
            cboEvaPropicepcion.Name = "N009-OTM00000059"; 
            cboPruebaMancuerda.Name = "N009-OTM00000060"; 
            cboRotulianoIzq.Name = "N009-OTM00000061"; 
            cboRotulianoDer.Name = "N009-OTM00000062"; 
            cboLasegueIzq.Name = "N009-OTM00000063"; 
            cboSchoverIzq.Name = "N009-OTM00000064"; 
            cboSchoverDer.Name = "N009-OTM00000065"; 
            cboLasegueDer.Name = "N009-OTM00000066"; 
            cboPhalenIzq.Name = "N009-OTM00000067"; 
            cboTinelIzq.Name = "N009-OTM00000068"; 
            cboTinelDer.Name = "N009-OTM00000069"; 
            cboPhalenDer.Name = "N009-OTM00000070"; 
            cboPalpacionDolor.Name = "N009-OTM00000071"; 
            cboPalpacionContractura.Name = "N009-OTM00000072"; 
            cboPalpacionApofisis.Name = "N009-OTM00000073"; 
            txtPalpacionDolor.Name = "N009-OTM00000074"; 
            txtPalpacionContractura.Name = "N009-OTM00000075"; 
            txtPalpacionApofisis.Name = "N009-OTM00000076"; 
            cboLumbatExtension.Name = "N009-OTM00000077"; 
            cboLumbatFlexion.Name = "N009-OTM00000078"; 
            cboCervicalExtension.Name = "N009-OTM00000079"; 
            cboCervicalFlexion.Name = "N009-OTM00000080"; 
            cboLumbatLateIzquierda.Name = "N009-OTM00000081"; 
            cboLumbatLateDerecha.Name = "N009-OTM00000082"; 
            cboLumbatRotacionIzquierda.Name = "N009-OTM00000083"; 
            cboLumbatRotacionDerecha.Name = "N009-OTM00000084"; 
            cboCervicalLateIzquierda.Name = "N009-OTM00000085"; 
            cboCervicalLateDerecha.Name = "N009-OTM00000086"; 
            cboCervicalRotaIzquierda.Name = "N009-OTM00000087"; 
            cboCervicalRotaDerecha.Name = "N009-OTM00000088"; 
            cboLumbatIrradiacion.Name = "N009-OTM00000089"; 
            cboCervicalIrradiacion.Name = "N009-OTM00000090"; 
            cboAsimetriaEscoliosis.Name = "N009-OTM00000091"; 
            cboAsimetriaHombros.Name = "N009-OTM00000092"; 
            cboAsimetriaLumbar.Name = "N009-OTM00000093"; 
            cboAsimetriaCaderas.Name = "N009-OTM00000094"; 
            cboAsimetriaHipercifocis.Name = "N009-OTM00000095"; 
            cboAsimetriaRodillas.Name = "N009-OTM00000096"; 
            cboEquilibrioLateralIzquierdo.Name = "N009-OTM00000097"; 
            cboEquilibrioLateralDerecho.Name = "N009-OTM00000098"; 
            cboEquilibrioPosterior.Name = "N009-OTM00000099"; 
            cboEquilibrioAnterior.Name = "N009-OTM00000100"; 
            cboMarchaClaudicacion.Name = "N009-OTM00000101"; 
            cboTobilloIzqRotInt.Name = "N009-OTM00000102"; 
            cboRodillaIzqRotInt.Name = "N009-OTM00000103"; 
            cboCaderaIzqRotInt.Name = "N009-OTM00000104"; 
            cboMunecaIzqRotInt.Name = "N009-OTM00000105"; 
            cboCodoIzqRotInt.Name = "N009-OTM00000106"; 
            cboHombroIzqRotInt.Name = "N009-OTM00000107"; 
            cboTobilloIzqExtension.Name = "N009-OTM00000108"; 
            cboRodillaIzqExtension.Name = "N009-OTM00000109"; 
            cboCaderaIzqExtension.Name = "N009-OTM00000110"; 
            cboMunecaIzqExtension.Name = "N009-OTM00000111"; 
            cboCodoIzqExtension.Name = "N009-OTM00000112"; 
            cboTobilloIzqFlexion.Name = "N009-OTM00000113"; 
            cboRodillaIzqFlexion.Name = "N009-OTM00000114"; 
            cboCaderaIzqFlexion.Name = "N009-OTM00000115"; 
            cboMunecaIzqFlexion.Name = "N009-OTM00000116"; 
            cboHombroIzqExtension.Name = "N009-OTM00000117"; 
            cboCodoIzqFlexion.Name = "N009-OTM00000118"; 
            cboTobilloDerRotInt.Name = "N009-OTM00000119"; 
            cboRodillaDerRotInt.Name = "N009-OTM00000120"; 
            cboCaderaDerRotInt.Name = "N009-OTM00000121"; 
            cboMunecaDerRotInt.Name = "N009-OTM00000122"; 
            cboHombroIzqFlexion.Name = "N009-OTM00000123"; 
            cboCodoDerRotInt.Name = "N009-OTM00000124"; 
            cboTobilloDerExtension.Name = "N009-OTM00000125"; 
            cboRodillaDerExtension.Name = "N009-OTM00000126"; 
            cboCaderaDerExtension.Name = "N009-OTM00000127"; 
            cboMunecaDerExtension.Name = "N009-OTM00000128"; 
            cboHombroDerRotInt.Name = "N009-OTM00000129"; 
            cboCodoDerExtension.Name = "N009-OTM00000130"; 
            cboTobilloDerFlexion.Name = "N009-OTM00000131"; 
            cboRodillaDerFlexion.Name = "N009-OTM00000132"; 
            cboCaderaDerFlexion.Name = "N009-OTM00000133"; 
            cboMunecaDerFlexion.Name = "N009-OTM00000134"; 
            cboHombroDerExtension.Name = "N009-OTM00000135"; 
            cboCodoDerFlexion.Name = "N009-OTM00000136"; 
            cboHombroDerFlexion.Name = "N009-OTM00000137"; 
            cboTobilloIzqTono.Name = "N009-OTM00000138"; 
            cboRodillaIzqTono.Name = "N009-OTM00000139"; 
            cboCaderaIzqTono.Name = "N009-OTM00000140"; 
            cboTobilloIzqFuerza.Name = "N009-OTM00000141"; 
            cboMunecaIzqTono.Name = "N009-OTM00000142"; 
            cboRodillaIzqFuerza.Name = "N009-OTM00000143"; 
            cboCaderaIzqFuerza.Name = "N009-OTM00000144"; 
            cboCodoIzqTono.Name = "N009-OTM00000145"; 
            cboTobilloIzqAbduccion.Name = "N009-OTM00000146"; 
            cboMunecaIzqFuerza.Name = "N009-OTM00000147"; 
            cboRodillaIzqAbduccion.Name = "N009-OTM00000148"; 
            cboCaderaIzqAbduccion.Name = "N009-OTM00000149"; 
            cboCodoIzqFuerza.Name = "N009-OTM00000150"; 
            cboTobilloIzqAduccion.Name = "N009-OTM00000151"; 
            cboMunecaIzqAbduccion.Name = "N009-OTM00000152"; 
            cboRodillaIzqAduccion.Name = "N009-OTM00000153"; 
            cboHombroIzqTono.Name = "N009-OTM00000154"; 
            cboCaderaIzqAduccion.Name = "N009-OTM00000155"; 
            cboCodoIzqAbduccion.Name = "N009-OTM00000156"; 
            cboTobilloIzqRotExt.Name = "N009-OTM00000157"; 
            cboMunecaIzqAduccion.Name = "N009-OTM00000158"; 
            cboRodillaIzqRotExt.Name = "N009-OTM00000159"; 
            cboHombroIzqFuerza.Name = "N009-OTM00000160"; 
            cboCaderaIzqRotExt.Name = "N009-OTM00000161"; 
            cboCodoIzqAduccion.Name = "N009-OTM00000162"; 
            cboTobilloDerTono.Name = "N009-OTM00000163"; 
            cboMunecaIzqRotExt.Name = "N009-OTM00000164"; 
            cboRodillaDerTono.Name = "N009-OTM00000165"; 
            cboHombroIzqAbduccion.Name = "N009-OTM00000166"; 
            cboCaderaDerTono.Name = "N009-OTM00000167"; 
            cboCodoIzqRotExt.Name = "N009-OTM00000168"; 
            cboTobilloDerFuerza.Name = "N009-OTM00000169"; 
            cboMunecaDerTono.Name = "N009-OTM00000170"; 
            cboRodillaDerFuerza.Name = "N009-OTM00000171"; 
            cboHombroIzqAduccion.Name = "N009-OTM00000172"; 
            cboCaderaDerFuerza.Name = "N009-OTM00000173"; 
            cboCodoDerTono.Name = "N009-OTM00000174"; 
            cboTobilloDerAbduccion.Name = "N009-OTM00000175"; 
            cboMunecaDerFuerza.Name = "N009-OTM00000176"; 
            cboRodillaDerAbduccion.Name = "N009-OTM00000177"; 
            cboHombroIzqRotExt.Name = "N009-OTM00000178"; 
            cboCaderaDerAbduccion.Name = "N009-OTM00000179"; 
            cboCodoDerFuerza.Name = "N009-OTM00000180"; 
            cboTobilloDerAduccion.Name = "N009-OTM00000181"; 
            cboMunecaDerAbduccion.Name = "N009-OTM00000182"; 
            cboRodillaDerAduccion.Name = "N009-OTM00000183"; 
            cboHombroDerTono.Name = "N009-OTM00000184"; 
            cboCaderaDerAduccion.Name = "N009-OTM00000185"; 
            cboCodoDerAbduccion.Name = "N009-OTM00000186"; 
            cboTobilloDerRotExt.Name = "N009-OTM00000187"; 
            cboMunecaDerAduccion.Name = "N009-OTM00000189"; 
            cboRodillaDerRotExt.Name = "N009-OTM00000190"; 
            cboHombroDerFuerza.Name = "N009-OTM00000191"; 
            cboCaderaDerRotExt.Name = "N009-OTM00000192"; 
            cboCodoDerAduccion.Name = "N009-OTM00000193"; 
            cboTobilloIzqDolor.Name = "N009-OTM00000194"; 
            cboMunecaDerRotExt.Name = "N009-OTM00000195"; 
            cboRodillaIzqDolor.Name = "N009-OTM00000196"; 
            cboHombroDerAbduccion.Name = "N009-OTM00000197"; 
            cboCaderaIzqDolor.Name = "N009-OTM00000198"; 
            cboCodoDerRotExt.Name = "N009-OTM00000221"; 
            cboTobilloDerDolor.Name = "N009-OTM00000200"; 
            cboMunecaIzqDolor.Name = "N009-OTM00000201"; 
            cboRodillaDerDolor.Name = "N009-OTM00000202"; 
            cboHombroDerAduccion.Name = "N009-OTM00000203"; 
            cboCaderaDerDolor.Name = "N009-OTM00000204"; 
            cboCodoIzqDolor.Name = "N009-OTM00000205"; 
            cboMunecaDerDolor.Name = "N009-OTM00000206"; 
            cboHombroDerRotExt.Name = "N009-OTM00000207"; 
            cboCodoDerDolor.Name = "N009-OTM00000208"; 
            cboHombroIzqDolor.Name = "N009-OTM00000209"; 
            cboHombroDerDolor.Name = "N009-OTM00000210"; 
            cboCodoIzqSupinacion.Name = "N009-OTM00000211"; 
            cboCodoIzqPronacion.Name = "N009-OTM00000212"; 
            cboCodoDerSupinacion.Name = "N009-OTM00000213"; 
            cboCodoDerPronacion.Name = "N009-OTM00000214"; 
            cboMunecaIzqRadial.Name = "N009-OTM00000215"; 
            cboMunecaIzqCubital.Name = "N009-OTM00000216"; 
            cboMunecaDerRadial.Name = "N009-OTM00000217"; 
            cboMunecaDerCubital.Name = "N009-OTM00000218"; 
            cboMunecaIzqInversion.Name = "N009-OTM00000219"; 
            cboMunecaIzqEversion.Name = "N009-OTM00000220"; 
            cboMunecaDerInversion.Name = "N009-OTM00000221"; 
            cboMunecaDerEversion.Name = "N009-OTM00000222"; 
            txtDescripcionHallazgos.Name = "N009-OTM00000223"; 
            cboConclusiones.Name = "N009-OTM00000224"; 


            rbAbdomenExcelente.Name = "N009-OTM00000274";
            rbAbdomenPromedio.Name = "N009-OTM00000274";
            rbAbdomenRegular.Name = "N009-OTM00000274";
            rbAbdomenPobre.Name = "N009-OTM00000274";
            txtAbdomenPuntos.Name = "N009-OTM00000228";
            txtAbdomenObservaciones.Name = "N009-OTM00000229";

            rbCaderaExcelente.Name = "N009-OTM00000230";
            rbCaderaPromedio.Name = "N009-OTM00000230";
            rbCaderaRegular.Name = "N009-OTM00000230";
            rbCaderaPobre.Name = "N009-OTM00000230";
            txtCaderaPuntos.Name = "N009-OTM00000234";
            txtCaderaOnservaciones.Name = "N009-OTM00000235";

            rbMusloExcelente.Name = "N009-OTM00000236";
            rbMusloPromedio.Name = "N009-OTM00000236";
            rbMusloRegular.Name = "N009-OTM00000236";
            rbMusloPobre.Name = "N009-OTM00000236";
            txtMusloPuntos.Name = "N009-OTM00000240";
            txtMusloObservaciones.Name = "N009-OTM00000241";

            rbAbdomenLateralExcelente.Name = "N009-OTM00000242";
            rbAbdomenLateralPromedio.Name = "N009-OTM00000242";
            rbAbdomenLateralRegular.Name = "N009-OTM00000242";
            rbAbdomenLateralPobre.Name = "N009-OTM00000242";
            txtAbdomenLateralPuntos.Name = "N009-OTM00000246";
            txtAbdomenLateralObservaciones.Name = "N009-OTM00000247";

            rbAbduccion180Optimo.Name = "N009-OTM00000248";
            rbAbduccion180Limitado.Name = "N009-OTM00000248";
            rbAbduccion180MuyLimitado.Name = "N009-OTM00000248";
            txtAbduccion180Puntos.Name = "N009-OTM00000249";
            rbAbduccion180DolorSI.Name = "N009-OTM00000252";
            rbAbduccion180DolorNO.Name = "N009-OTM00000252";

            rbAbduccion60Optimo.Name = "N009-OTM00000254";
            rbAbduccion60Limitado.Name = "N009-OTM00000254";
            rbAbduccion60MuyLimitado.Name = "N009-OTM00000254";
            txtAbduccion60Puntos.Name = "N009-OTM00000257";
            rbAbduccion60DolorSI.Name = "N009-OTM00000258";
            rbAbduccion60DolorNO.Name = "N009-OTM00000258";

            rbRotacion090Optimo.Name = "N009-OTM00000260";
            rbRotacion090Limitado.Name = "N009-OTM00000260";
            rbRotacion090MuyLimitado.Name = "N009-OTM00000260";
            txtRotacion090Puntos.Name = "N009-OTM00000263";
            rbRotacion090DolorSI.Name = "N009-OTM00000264";
            rbRotacion090DolorNO.Name = "N009-OTM00000264";

            rbRotacionExtIntOptimo.Name = "N009-OTM00000266";
            rbRotacionExtIntLimitado.Name = "N009-OTM00000266";
            rbRotacionExtIntMuyLimitado.Name = "N009-OTM00000266";
            txtRotacionExtIntPuntos.Name = "N009-OTM00000269";
            rbRotacionExtIntDolorSI.Name = "N009-OTM00000270";
            rbRotacionExtIntDolorNO.Name = "N009-OTM00000270";
            
            txtTotalAptitudEspalda.Name = "N009-OTM00000272";

            txtTotalRangos.Name = "N009-OTM00000273";

            #region BindCombos
            OperationResult objOperationResult = new OperationResult();
            SystemParameterBL objSystemParameterBl = new SystemParameterBL();

            Utils.LoadDropDownList(cboCondiAmbEspacio, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000002"; 
            Utils.LoadDropDownList(cboCondiAmbTemperatura, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000003"; 
            Utils.LoadDropDownList(cboCondiAmbVibraciones, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000004"; 
            Utils.LoadDropDownList(cboCondiAmbSueloInestable, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000005"; 
            Utils.LoadDropDownList(cboCondiAmbDesniveles, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000006"; 
            Utils.LoadDropDownList(cboCondiAmbAltura, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000007"; 
            Utils.LoadDropDownList(cboCondiAmbPostura, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000008"; 
            Utils.LoadDropDownList(cboCondiAmbSuelo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000009"; 
            Utils.LoadDropDownList(cboExigRitmo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000010"; 
            Utils.LoadDropDownList(cboExigDistancias, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000011"; 
            Utils.LoadDropDownList(cboExigPeriodo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000012"; 
            Utils.LoadDropDownList(cboExigEsfuerzo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000013"; 
            Utils.LoadDropDownList(cboEsfFisAlzar, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000014"; 
            Utils.LoadDropDownList(cboEsfFisExiste, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000015"; 
            Utils.LoadDropDownList(cboEsfFisCuerpo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000016"; 
            Utils.LoadDropDownList(cboEsfFisExige, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000017"; 
            Utils.LoadDropDownList(cboCarCargaPeso, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000018"; 
            Utils.LoadDropDownList(cboCarCargaManipulacion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000019"; 
            Utils.LoadDropDownList(cboCarCargaEquilibrio, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000020"; 
            Utils.LoadDropDownList(cboCarCargaVolumen, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000021"; 
            Utils.LoadDropDownList(cboExpToxCadmio, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000022"; 
            Utils.LoadDropDownList(cboExpToxMercurio, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000023"; 
            Utils.LoadDropDownList(cboExpToxMagneso, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000024"; 
            Utils.LoadDropDownList(cbo2RiesgoLevanta, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000031"; 
            Utils.LoadDropDownList(cbo2RiesgoDesplaza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000032"; 
            Utils.LoadDropDownList(cbo2RiesgoSentado, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000033"; 
            Utils.LoadDropDownList(cbo2RiesgoPie, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000034"; 
            Utils.LoadDropDownList(cbo2RiesgoTracciona, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000035"; 
            Utils.LoadDropDownList(cbo2RiesgoEmpuja, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000036"; 
            Utils.LoadDropDownList(cbo2RiesgoColoca, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000037"; 
            Utils.LoadDropDownList(cboEjeLordosisCervical, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 200), DropDownListAction.Select);// = "N009-OTM00000038"; 
            Utils.LoadDropDownList(cboEjeCifosisDorsal, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 200), DropDownListAction.Select);// = "N009-OTM00000039"; 
            Utils.LoadDropDownList(cboEjeLordosisLumbar, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 200), DropDownListAction.Select);// = "N009-OTM00000040"; 
            Utils.LoadDropDownList(cboRotacionExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000041"; 
            Utils.LoadDropDownList(cboRotacionInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000042"; 
            Utils.LoadDropDownList(cboInversion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000043"; 
            Utils.LoadDropDownList(cboPlantiflexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000044"; 
            Utils.LoadDropDownList(cboEversion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000045"; 
            Utils.LoadDropDownList(cboDorsoflexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000046"; 
            Utils.LoadDropDownList(cboMmiiFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000047"; 
            Utils.LoadDropDownList(cboMmiiExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000048"; 
            Utils.LoadDropDownList(cboMmiiContraResistencia, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000049"; 
            Utils.LoadDropDownList(cboCircunduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000050"; 
            Utils.LoadDropDownList(cboDesviacionRadial, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000051"; 
            Utils.LoadDropDownList(cboExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000052"; 
            Utils.LoadDropDownList(cboDesviacionCubital, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000053"; 
            Utils.LoadDropDownList(cboFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000054"; 
            Utils.LoadDropDownList(cboGenuvaro, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000055"; 
            Utils.LoadDropDownList(cboGenuvalgo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000056"; 
            Utils.LoadDropDownList(cboPieCavo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000057"; 
            Utils.LoadDropDownList(cboPiePlano, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000058"; 
            Utils.LoadDropDownList(cboEvaPropicepcion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000059"; 
            Utils.LoadDropDownList(cboPruebaMancuerda, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 247), DropDownListAction.Select);// = "N009-OTM00000060"; 
            Utils.LoadDropDownList(cboRotulianoIzq, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000061"; 
            Utils.LoadDropDownList(cboRotulianoDer, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000062"; 
            Utils.LoadDropDownList(cboLasegueIzq, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);// = "N009-OTM00000063"; 
            Utils.LoadDropDownList(cboSchoverIzq, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);// = "N009-OTM00000064"; 
            Utils.LoadDropDownList(cboSchoverDer, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);// = "N009-OTM00000065"; 
            Utils.LoadDropDownList(cboLasegueDer, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);// = "N009-OTM00000066"; 
            Utils.LoadDropDownList(cboPhalenIzq, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);// = "N009-OTM00000067"; 
            Utils.LoadDropDownList(cboTinelIzq, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);// = "N009-OTM00000068"; 
            Utils.LoadDropDownList(cboTinelDer, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);// = "N009-OTM00000069"; 
            Utils.LoadDropDownList(cboPhalenDer, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 203), DropDownListAction.Select);// = "N009-OTM00000070"; 
            Utils.LoadDropDownList(cboPalpacionDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000071"; 
            Utils.LoadDropDownList(cboPalpacionContractura, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000072"; 
            Utils.LoadDropDownList(cboPalpacionApofisis, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000073"; 
            Utils.LoadDropDownList(cboLumbatExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000077"; 
            Utils.LoadDropDownList(cboLumbatFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000078"; 
            Utils.LoadDropDownList(cboLumbatLateIzquierda, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000081"; 
            Utils.LoadDropDownList(cboLumbatLateDerecha, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000082"; 
            Utils.LoadDropDownList(cboLumbatRotacionIzquierda, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000083"; 
            Utils.LoadDropDownList(cboLumbatRotacionDerecha, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000084"; 
           
            Utils.LoadDropDownList(cboCervicalExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000079"; 
            Utils.LoadDropDownList(cboCervicalFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000080"; 
            Utils.LoadDropDownList(cboCervicalLateIzquierda, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000085"; 
            Utils.LoadDropDownList(cboCervicalLateDerecha, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000086"; 
            Utils.LoadDropDownList(cboCervicalRotaIzquierda, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000087"; 
            Utils.LoadDropDownList(cboCervicalRotaDerecha, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000088"; 
            Utils.LoadDropDownList(cboCervicalIrradiacion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000090"; 
            Utils.LoadDropDownList(cboEquilibrioLateralIzquierdo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000097"; 
            Utils.LoadDropDownList(cboEquilibrioLateralDerecho, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000098"; 
            Utils.LoadDropDownList(cboEquilibrioPosterior, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000099"; 
            Utils.LoadDropDownList(cboEquilibrioAnterior, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000100"; 
           

            Utils.LoadDropDownList(cboLumbatIrradiacion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000089"; 
            Utils.LoadDropDownList(cboAsimetriaEscoliosis, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000091"; 
            Utils.LoadDropDownList(cboAsimetriaHombros, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000092"; 
            Utils.LoadDropDownList(cboAsimetriaLumbar, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000093"; 
            Utils.LoadDropDownList(cboAsimetriaCaderas, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000094"; 
            Utils.LoadDropDownList(cboAsimetriaHipercifocis, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000095"; 
            Utils.LoadDropDownList(cboAsimetriaRodillas, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000096"; 
            Utils.LoadDropDownList(cboMarchaClaudicacion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);// = "N009-OTM00000101"; 
            Utils.LoadDropDownList(cboTobilloIzqRotInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000102"; 
            Utils.LoadDropDownList(cboRodillaIzqRotInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000103"; 
            Utils.LoadDropDownList(cboCaderaIzqRotInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000104"; 
            Utils.LoadDropDownList(cboMunecaIzqRotInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000105"; 
            Utils.LoadDropDownList(cboCodoIzqRotInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000106"; 
            Utils.LoadDropDownList(cboHombroIzqRotInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000107"; 
            Utils.LoadDropDownList(cboTobilloIzqExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000108"; 
            Utils.LoadDropDownList(cboRodillaIzqExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000109"; 
            Utils.LoadDropDownList(cboCaderaIzqExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000110"; 
            Utils.LoadDropDownList(cboMunecaIzqExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000111"; 
            Utils.LoadDropDownList(cboCodoIzqExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000112"; 
            Utils.LoadDropDownList(cboTobilloIzqFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000113"; 
            Utils.LoadDropDownList(cboRodillaIzqFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000114"; 
            Utils.LoadDropDownList(cboCaderaIzqFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000115"; 
            Utils.LoadDropDownList(cboMunecaIzqFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000116"; 
            Utils.LoadDropDownList(cboHombroIzqExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000117"; 
            Utils.LoadDropDownList(cboCodoIzqFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000118"; 
            Utils.LoadDropDownList(cboTobilloDerRotInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000119"; 
            Utils.LoadDropDownList(cboRodillaDerRotInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000120"; 
            Utils.LoadDropDownList(cboCaderaDerRotInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000121"; 
            Utils.LoadDropDownList(cboMunecaDerRotInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000122"; 
            Utils.LoadDropDownList(cboHombroIzqFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000123"; 
            Utils.LoadDropDownList(cboCodoDerRotInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000124"; 
            Utils.LoadDropDownList(cboTobilloDerExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000125"; 
            Utils.LoadDropDownList(cboRodillaDerExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000126"; 
            Utils.LoadDropDownList(cboCaderaDerExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000127"; 
            Utils.LoadDropDownList(cboMunecaDerExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000128"; 
            Utils.LoadDropDownList(cboHombroDerRotInt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000129"; 
            Utils.LoadDropDownList(cboCodoDerExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000130"; 
            Utils.LoadDropDownList(cboTobilloDerFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000131"; 
            Utils.LoadDropDownList(cboRodillaDerFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000132"; 
            Utils.LoadDropDownList(cboCaderaDerFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000133"; 
            Utils.LoadDropDownList(cboMunecaDerFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000134"; 
            Utils.LoadDropDownList(cboHombroDerExtension, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000135"; 
            Utils.LoadDropDownList(cboCodoDerFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000136"; 
            
            Utils.LoadDropDownList(cboHombroDerFlexion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000137"; 
            Utils.LoadDropDownList(cboTobilloIzqTono, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000138"; 
            Utils.LoadDropDownList(cboRodillaIzqTono, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000139"; 
            Utils.LoadDropDownList(cboCaderaIzqTono, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000140"; 
            Utils.LoadDropDownList(cboTobilloIzqFuerza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000141"; 
            Utils.LoadDropDownList(cboMunecaIzqTono, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000142"; 
            Utils.LoadDropDownList(cboRodillaIzqFuerza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000143"; 
            Utils.LoadDropDownList(cboCaderaIzqFuerza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000144"; 
            Utils.LoadDropDownList(cboCodoIzqTono, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000145"; 
            Utils.LoadDropDownList(cboTobilloIzqAbduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000146"; 
            Utils.LoadDropDownList(cboMunecaIzqFuerza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000147"; 
            Utils.LoadDropDownList(cboRodillaIzqAbduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000148"; 
            Utils.LoadDropDownList(cboCaderaIzqAbduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000149"; 
            Utils.LoadDropDownList(cboCodoIzqFuerza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000150"; 
            Utils.LoadDropDownList(cboTobilloIzqAduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000151"; 
            Utils.LoadDropDownList(cboMunecaIzqAbduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000152"; 
            Utils.LoadDropDownList(cboRodillaIzqAduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000153"; 
            Utils.LoadDropDownList(cboHombroIzqTono, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000154"; 
            Utils.LoadDropDownList(cboCaderaIzqAduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000155"; 
            Utils.LoadDropDownList(cboCodoIzqAbduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000156"; 
            Utils.LoadDropDownList(cboTobilloIzqRotExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000157"; 
            Utils.LoadDropDownList(cboMunecaIzqAduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000158"; 
            Utils.LoadDropDownList(cboRodillaIzqRotExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000159"; 
            Utils.LoadDropDownList(cboHombroIzqFuerza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000160"; 
            Utils.LoadDropDownList(cboCaderaIzqRotExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000161"; 
            Utils.LoadDropDownList(cboCodoIzqAduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000162"; 
            Utils.LoadDropDownList(cboTobilloDerTono, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000163"; 
            Utils.LoadDropDownList(cboMunecaIzqRotExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000164"; 
            Utils.LoadDropDownList(cboRodillaDerTono, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000165"; 
            Utils.LoadDropDownList(cboHombroIzqAbduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000166"; 
            Utils.LoadDropDownList(cboCaderaDerTono, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000167"; 
            Utils.LoadDropDownList(cboCodoIzqRotExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000168"; 
            Utils.LoadDropDownList(cboTobilloDerFuerza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000169"; 
            Utils.LoadDropDownList(cboMunecaDerTono, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000170"; 
            Utils.LoadDropDownList(cboRodillaDerFuerza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000171"; 
            Utils.LoadDropDownList(cboHombroIzqAduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000172"; 
            Utils.LoadDropDownList(cboCaderaDerFuerza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000173"; 
            Utils.LoadDropDownList(cboCodoDerTono, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000174"; 
            Utils.LoadDropDownList(cboTobilloDerAbduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000175"; 
            Utils.LoadDropDownList(cboMunecaDerFuerza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000176"; 
            Utils.LoadDropDownList(cboRodillaDerAbduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000177"; 
            Utils.LoadDropDownList(cboHombroIzqRotExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000178"; 
            Utils.LoadDropDownList(cboCaderaDerAbduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000179"; 
            Utils.LoadDropDownList(cboCodoDerFuerza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000180"; 
            Utils.LoadDropDownList(cboTobilloDerAduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000181"; 
            Utils.LoadDropDownList(cboMunecaDerAbduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000182"; 
            Utils.LoadDropDownList(cboRodillaDerAduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000183"; 
            Utils.LoadDropDownList(cboHombroDerTono, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000184"; 
            Utils.LoadDropDownList(cboCaderaDerAduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000185"; 
            Utils.LoadDropDownList(cboCodoDerAbduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000186"; 
            Utils.LoadDropDownList(cboTobilloDerRotExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000187"; 
            Utils.LoadDropDownList(cboMunecaDerAduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000189"; 
            Utils.LoadDropDownList(cboRodillaDerRotExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000190"; 
            Utils.LoadDropDownList(cboHombroDerFuerza, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000191"; 
            Utils.LoadDropDownList(cboCaderaDerRotExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000192"; 
            Utils.LoadDropDownList(cboCodoDerAduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000193"; 
            Utils.LoadDropDownList(cboTobilloIzqDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000194"; 

            Utils.LoadDropDownList(cboMunecaDerRotExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000195"; 
            Utils.LoadDropDownList(cboRodillaIzqDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000196"; 
            Utils.LoadDropDownList(cboHombroDerAbduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000197"; 
            Utils.LoadDropDownList(cboCaderaIzqDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000198"; 
            Utils.LoadDropDownList(cboCodoDerRotExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000221"; 
            Utils.LoadDropDownList(cboTobilloDerDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000200"; 
            Utils.LoadDropDownList(cboMunecaIzqDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000201"; 
            Utils.LoadDropDownList(cboRodillaDerDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000202"; 
            Utils.LoadDropDownList(cboHombroDerAduccion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000203"; 
            Utils.LoadDropDownList(cboCaderaDerDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000204"; 
            Utils.LoadDropDownList(cboCodoIzqDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000205"; 
            Utils.LoadDropDownList(cboMunecaDerDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000206"; 
            Utils.LoadDropDownList(cboHombroDerRotExt, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000207"; 
            Utils.LoadDropDownList(cboCodoDerDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000208"; 
            Utils.LoadDropDownList(cboHombroIzqDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000209"; 
            Utils.LoadDropDownList(cboHombroDerDolor, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000210"; 
            Utils.LoadDropDownList(cboCodoIzqSupinacion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000211"; 
            Utils.LoadDropDownList(cboCodoIzqPronacion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000212"; 
            Utils.LoadDropDownList(cboCodoDerSupinacion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000213"; 
            Utils.LoadDropDownList(cboCodoDerPronacion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000214"; 
            Utils.LoadDropDownList(cboMunecaIzqRadial, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000215"; 
            Utils.LoadDropDownList(cboMunecaIzqCubital, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000216"; 
            Utils.LoadDropDownList(cboMunecaDerRadial, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000217"; 
            Utils.LoadDropDownList(cboMunecaDerCubital, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000218"; 
            Utils.LoadDropDownList(cboMunecaIzqInversion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000219"; 
            Utils.LoadDropDownList(cboMunecaIzqEversion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000220"; 
            Utils.LoadDropDownList(cboMunecaDerInversion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000221"; 
            Utils.LoadDropDownList(cboMunecaDerEversion, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000222"; 
            Utils.LoadDropDownList(cboConclusiones, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 221), DropDownListAction.Select);// = "N009-OTM00000224"; 


            cbo2RiesgoLevanta.SelectedValue = "0";//  = "N009-OTM00000031";
            cbo2RiesgoDesplaza.SelectedValue = "0";//  = "N009-OTM00000032";
            cbo2RiesgoSentado.SelectedValue = "0";//  = "N009-OTM00000033";
            cbo2RiesgoPie.SelectedValue = "0";//  = "N009-OTM00000034";
            cbo2RiesgoTracciona.SelectedValue = "0";//  = "N009-OTM00000035";
            cbo2RiesgoEmpuja.SelectedValue = "0";//  = "N009-OTM00000036";
            cbo2RiesgoColoca.SelectedValue = "0";//  = "N009-OTM00000037";

            cboExpToxCadmio.SelectedValue = "0";//  = "N009-OTM00000022";
            cboExpToxMercurio.SelectedValue = "0";//  = "N009-OTM00000023";
            cboExpToxMagneso.SelectedValue = "0";//  = "N009-OTM00000024";

            cboCarCargaPeso.SelectedValue = "0";//  = "N009-OTM00000018";
            cboCarCargaManipulacion.SelectedValue = "0";//  = "N009-OTM00000019";
            cboCarCargaEquilibrio.SelectedValue = "0";//  = "N009-OTM00000020";
            cboCarCargaVolumen.SelectedValue = "0";//  = "N009-OTM00000021";

            cboCondiAmbEspacio.SelectedValue = "0";// .Name = "N009-OTM00000002";
            cboCondiAmbTemperatura.SelectedValue = "0";//  = "N009-OTM00000003";
            cboCondiAmbVibraciones.SelectedValue = "0";//  = "N009-OTM00000004";
            cboCondiAmbSueloInestable.SelectedValue = "0";//  = "N009-OTM00000005";
            cboCondiAmbDesniveles.SelectedValue = "0";//  = "N009-OTM00000006";
            cboCondiAmbAltura.SelectedValue = "0";//  = "N009-OTM00000007";
            cboCondiAmbPostura.SelectedValue = "0";//  = "N009-OTM00000008";
            cboCondiAmbSuelo.SelectedValue = "0";//  = "N009-OTM00000009";
            
            cboEsfFisAlzar.SelectedValue = "0";//  = "N009-OTM00000014";
            cboEsfFisExiste.SelectedValue = "0";//  = "N009-OTM00000015";
            cboEsfFisCuerpo.SelectedValue = "0";//  = "N009-OTM00000016";
            cboEsfFisExige.SelectedValue = "0";//  = "N009-OTM00000017";
            
            cboExigRitmo.SelectedValue = "0";//  = "N009-OTM00000010";
            cboExigDistancias.SelectedValue = "0";//  = "N009-OTM00000011";
            cboExigPeriodo.SelectedValue = "0";//  = "N009-OTM00000012";
            cboExigEsfuerzo.SelectedValue = "0";//  = "N009-OTM00000013";
           
            cboEjeLordosisCervical.SelectedValue = "1";//  = "N009-OTM00000038";
            cboEjeCifosisDorsal.SelectedValue = "1";//  = "N009-OTM00000039";
            cboEjeLordosisLumbar.SelectedValue = "1";//  = "N009-OTM00000040";

            cboAsimetriaEscoliosis.SelectedValue = "0";//  = "N009-OTM00000091";
            cboAsimetriaHombros.SelectedValue = "0";//  = "N009-OTM00000092";
            cboAsimetriaLumbar.SelectedValue = "0";//  = "N009-OTM00000093";
            cboAsimetriaCaderas.SelectedValue = "0";//  = "N009-OTM00000094";
            cboAsimetriaHipercifocis.SelectedValue = "0";//  = "N009-OTM00000095";
            cboAsimetriaRodillas.SelectedValue = "0";//  = "N009-OTM00000096";

            cboPalpacionDolor.SelectedValue = "0";//  = "N009-OTM00000071";
            cboPalpacionContractura.SelectedValue = "0";//  = "N009-OTM00000072";
            cboPalpacionApofisis.SelectedValue = "0";//  = "N009-OTM00000073";

            cboPhalenIzq.SelectedValue = "2";//  = "N009-OTM00000067";
            cboPhalenDer.SelectedValue = "2";//  = "N009-OTM00000070";

            cboTinelIzq.SelectedValue = "2";//  = "N009-OTM00000068";
            cboTinelDer.SelectedValue = "2";//  = "N009-OTM00000069";

            cboEvaPropicepcion.SelectedValue = "1";//  = "N009-OTM00000059";
            cboPruebaMancuerda.SelectedValue = "1";//  = "N009-OTM00000060";

            cboFlexion.SelectedValue = "1";//  = "N009-OTM00000054";
            cboExtension.SelectedValue = "1";//  = "N009-OTM00000052";
            cboDesviacionCubital.SelectedValue = "1";//  = "N009-OTM00000053";
            cboDesviacionRadial.SelectedValue = "1";//  = "N009-OTM00000051";
            cboCircunduccion.SelectedValue = "1";//  = "N009-OTM00000050";

            cboDorsoflexion.SelectedValue = "1";//  = "N009-OTM00000046";
            cboPlantiflexion.SelectedValue = "1";//  = "N009-OTM00000044";
            cboEversion.SelectedValue = "1";//  = "N009-OTM00000045";
            cboInversion.SelectedValue = "1";//  = "N009-OTM00000043";
            cboRotacionInt.SelectedValue = "1";//  = "N009-OTM00000042";
            cboRotacionExt.SelectedValue = "1";//  = "N009-OTM00000041";

            cboMmiiFlexion.SelectedValue = "1";//  = "N009-OTM00000047";
            cboMmiiExtension.SelectedValue = "1";//  = "N009-OTM00000048";
            cboMmiiContraResistencia.SelectedValue = "1";//  = "N009-OTM00000049";


            cboGenuvaro.SelectedValue = "0";//  = "N009-OTM00000055";
            cboGenuvalgo.SelectedValue = "0";//  = "N009-OTM00000056";
            cboPieCavo.SelectedValue = "0";//  = "N009-OTM00000057";
            cboPiePlano.SelectedValue = "0";//  = "N009-OTM00000058";
            
            
            
            
       
            
            
           
            
       


           
            cboRotulianoIzq.SelectedValue = "1";//  = "N009-OTM00000061";
            cboRotulianoDer.SelectedValue = "1";//  = "N009-OTM00000062";
            cboLasegueIzq.SelectedValue = "2";//  = "N009-OTM00000063";
            cboSchoverIzq.SelectedValue = "2";//  = "N009-OTM00000064";
            cboSchoverDer.SelectedValue = "2";//  = "N009-OTM00000065";
            cboLasegueDer.SelectedValue = "2";//  = "N009-OTM00000066";
          
           
           
            cboLumbatExtension.SelectedValue = "2";//  = "N009-OTM00000077";
            cboLumbatFlexion.SelectedValue = "2";//  = "N009-OTM00000078";
            cboCervicalExtension.SelectedValue = "2";//  = "N009-OTM00000079";
            cboCervicalFlexion.SelectedValue = "2";//  = "N009-OTM00000080";
            cboLumbatLateIzquierda.SelectedValue = "2";//  = "N009-OTM00000081";
            cboLumbatLateDerecha.SelectedValue = "2";//  = "N009-OTM00000082";
            cboLumbatRotacionIzquierda.SelectedValue = "2";//  = "N009-OTM00000083";
            cboLumbatRotacionDerecha.SelectedValue = "2";//  = "N009-OTM00000084";
            cboCervicalLateIzquierda.SelectedValue = "2";//  = "N009-OTM00000085";
            cboCervicalLateDerecha.SelectedValue = "2";//  = "N009-OTM00000086";
            cboCervicalRotaIzquierda.SelectedValue = "2";//  = "N009-OTM00000087";
            cboCervicalRotaDerecha.SelectedValue = "2";//  = "N009-OTM00000088";
            cboLumbatIrradiacion.SelectedValue = "2";//  = "N009-OTM00000089";
            cboCervicalIrradiacion.SelectedValue = "2";//  = "N009-OTM00000090";

            cboEquilibrioLateralIzquierdo.SelectedValue = "2";//  = "N009-OTM00000097";
            cboEquilibrioLateralDerecho.SelectedValue = "2";//  = "N009-OTM00000098";
            cboEquilibrioPosterior.SelectedValue = "2";//  = "N009-OTM00000099";
            cboEquilibrioAnterior.SelectedValue = "2";//  = "N009-OTM00000100";
            cboMarchaClaudicacion.SelectedValue = "0";//  = "N009-OTM00000101";
            cboTobilloIzqRotInt.SelectedValue = "2";//  = "N009-OTM00000102";
            cboRodillaIzqRotInt.SelectedValue = "2";//  = "N009-OTM00000103";
            cboCaderaIzqRotInt.SelectedValue = "2";//  = "N009-OTM00000104";
            cboMunecaIzqRotInt.SelectedValue = "2";//  = "N009-OTM00000105";
            cboCodoIzqRotInt.SelectedValue = "2";//  = "N009-OTM00000106";
            cboHombroIzqRotInt.SelectedValue = "2";//  = "N009-OTM00000107";
            cboTobilloIzqExtension.SelectedValue = "2";//  = "N009-OTM00000108";
            cboRodillaIzqExtension.SelectedValue = "2";//  = "N009-OTM00000109";
            cboCaderaIzqExtension.SelectedValue = "2";//  = "N009-OTM00000110";
            cboMunecaIzqExtension.SelectedValue = "2";//  = "N009-OTM00000111";
            cboCodoIzqExtension.SelectedValue = "2";//  = "N009-OTM00000112";
            cboTobilloIzqFlexion.SelectedValue = "2";//  = "N009-OTM00000113";
            cboRodillaIzqFlexion.SelectedValue = "2";//  = "N009-OTM00000114";
            cboCaderaIzqFlexion.SelectedValue = "2";//  = "N009-OTM00000115";
            cboMunecaIzqFlexion.SelectedValue = "2";//  = "N009-OTM00000116";
            cboHombroIzqExtension.SelectedValue = "2";//  = "N009-OTM00000117";
            cboCodoIzqFlexion.SelectedValue = "2";//  = "N009-OTM00000118";
            cboTobilloDerRotInt.SelectedValue = "2";//  = "N009-OTM00000119";
            cboRodillaDerRotInt.SelectedValue = "2";//  = "N009-OTM00000120";
            cboCaderaDerRotInt.SelectedValue = "2";//  = "N009-OTM00000121";
            cboMunecaDerRotInt.SelectedValue = "2";//  = "N009-OTM00000122";
            cboHombroIzqFlexion.SelectedValue = "2";//  = "N009-OTM00000123";
            cboCodoDerRotInt.SelectedValue = "2";//  = "N009-OTM00000124";
            cboTobilloDerExtension.SelectedValue = "2";//  = "N009-OTM00000125";
            cboRodillaDerExtension.SelectedValue = "2";//  = "N009-OTM00000126";
            cboCaderaDerExtension.SelectedValue = "2";//  = "N009-OTM00000127";
            cboMunecaDerExtension.SelectedValue = "2";//  = "N009-OTM00000128";
            cboHombroDerRotInt.SelectedValue = "2";//  = "N009-OTM00000129";
            cboCodoDerExtension.SelectedValue = "2";//  = "N009-OTM00000130";
            cboTobilloDerFlexion.SelectedValue = "2";//  = "N009-OTM00000131";
            cboRodillaDerFlexion.SelectedValue = "2";//  = "N009-OTM00000132";
            cboCaderaDerFlexion.SelectedValue = "2";//  = "N009-OTM00000133";
            cboMunecaDerFlexion.SelectedValue = "2";//  = "N009-OTM00000134";
            cboHombroDerExtension.SelectedValue = "2";//  = "N009-OTM00000135";
            cboCodoDerFlexion.SelectedValue = "2";//  = "N009-OTM00000136";
            cboHombroDerFlexion.SelectedValue = "2";//  = "N009-OTM00000137";
            cboTobilloIzqTono.SelectedValue = "2";//  = "N009-OTM00000138";
            cboRodillaIzqTono.SelectedValue = "2";//  = "N009-OTM00000139";
            cboCaderaIzqTono.SelectedValue = "2";//  = "N009-OTM00000140";
            cboTobilloIzqFuerza.SelectedValue = "2";//  = "N009-OTM00000141";
            cboMunecaIzqTono.SelectedValue = "2";//  = "N009-OTM00000142";
            cboRodillaIzqFuerza.SelectedValue = "2";//  = "N009-OTM00000143";
            cboCaderaIzqFuerza.SelectedValue = "2";//  = "N009-OTM00000144";
            cboCodoIzqTono.SelectedValue = "2";//  = "N009-OTM00000145";
            cboTobilloIzqAbduccion.SelectedValue = "2";//  = "N009-OTM00000146";
            cboMunecaIzqFuerza.SelectedValue = "2";//  = "N009-OTM00000147";
            cboRodillaIzqAbduccion.SelectedValue = "2";//  = "N009-OTM00000148";
            cboCaderaIzqAbduccion.SelectedValue = "2";//  = "N009-OTM00000149";
            cboCodoIzqFuerza.SelectedValue = "2";//  = "N009-OTM00000150";
            cboTobilloIzqAduccion.SelectedValue = "2";//  = "N009-OTM00000151";
            cboMunecaIzqAbduccion.SelectedValue = "2";//  = "N009-OTM00000152";
            cboRodillaIzqAduccion.SelectedValue = "2";//  = "N009-OTM00000153";
            cboHombroIzqTono.SelectedValue = "2";//  = "N009-OTM00000154";
            cboCaderaIzqAduccion.SelectedValue = "2";//  = "N009-OTM00000155";
            cboCodoIzqAbduccion.SelectedValue = "2";//  = "N009-OTM00000156";
            cboTobilloIzqRotExt.SelectedValue = "2";//  = "N009-OTM00000157";
            cboMunecaIzqAduccion.SelectedValue = "2";//  = "N009-OTM00000158";
            cboRodillaIzqRotExt.SelectedValue = "2";//  = "N009-OTM00000159";
            cboHombroIzqFuerza.SelectedValue = "2";//  = "N009-OTM00000160";
            cboCaderaIzqRotExt.SelectedValue = "2";//  = "N009-OTM00000161";
            cboCodoIzqAduccion.SelectedValue = "2";//  = "N009-OTM00000162";
            cboTobilloDerTono.SelectedValue = "2";//  = "N009-OTM00000163";
            cboMunecaIzqRotExt.SelectedValue = "2";//  = "N009-OTM00000164";
            cboRodillaDerTono.SelectedValue = "2";//  = "N009-OTM00000165";
            cboHombroIzqAbduccion.SelectedValue = "2";//  = "N009-OTM00000166";
            cboCaderaDerTono.SelectedValue = "2";//  = "N009-OTM00000167";
            cboCodoIzqRotExt.SelectedValue = "2";//  = "N009-OTM00000168";
            cboTobilloDerFuerza.SelectedValue = "2";//  = "N009-OTM00000169";
            cboMunecaDerTono.SelectedValue = "2";//  = "N009-OTM00000170";
            cboRodillaDerFuerza.SelectedValue = "2";//  = "N009-OTM00000171";
            cboHombroIzqAduccion.SelectedValue = "2";//  = "N009-OTM00000172";
            cboCaderaDerFuerza.SelectedValue = "2";//  = "N009-OTM00000173";
            cboCodoDerTono.SelectedValue = "2";//  = "N009-OTM00000174";
            cboTobilloDerAbduccion.SelectedValue = "2";//  = "N009-OTM00000175";
            cboMunecaDerFuerza.SelectedValue = "2";//  = "N009-OTM00000176";
            cboRodillaDerAbduccion.SelectedValue = "2";//  = "N009-OTM00000177";
            cboHombroIzqRotExt.SelectedValue = "2";//  = "N009-OTM00000178";
            cboCaderaDerAbduccion.SelectedValue = "2";//  = "N009-OTM00000179";
            cboCodoDerFuerza.SelectedValue = "2";//  = "N009-OTM00000180";
            cboTobilloDerAduccion.SelectedValue = "2";//  = "N009-OTM00000181";
            cboMunecaDerAbduccion.SelectedValue = "2";//  = "N009-OTM00000182";
            cboRodillaDerAduccion.SelectedValue = "2";//  = "N009-OTM00000183";
            cboHombroDerTono.SelectedValue = "2";//  = "N009-OTM00000184";
            cboCaderaDerAduccion.SelectedValue = "2";//  = "N009-OTM00000185";
            cboCodoDerAbduccion.SelectedValue = "2";//  = "N009-OTM00000186";
            cboTobilloDerRotExt.SelectedValue = "2";//  = "N009-OTM00000187";
            cboMunecaDerAduccion.SelectedValue = "2";//  = "N009-OTM00000189";
            cboRodillaDerRotExt.SelectedValue = "2";//  = "N009-OTM00000190";
            cboHombroDerFuerza.SelectedValue = "2";//  = "N009-OTM00000191";
            cboCaderaDerRotExt.SelectedValue = "2";//  = "N009-OTM00000192";
            cboCodoDerAduccion.SelectedValue = "2";//  = "N009-OTM00000193";
            cboTobilloIzqDolor.SelectedValue = "2";//  = "N009-OTM00000194";
            cboMunecaDerRotExt.SelectedValue = "2";//  = "N009-OTM00000195";
            cboRodillaIzqDolor.SelectedValue = "2";//  = "N009-OTM00000196";
            cboHombroDerAbduccion.SelectedValue = "2";//  = "N009-OTM00000197";
            cboCaderaIzqDolor.SelectedValue = "2";//  = "N009-OTM00000198";
            cboCodoDerRotExt.SelectedValue = "2";//  = "N009-OTM00000221";
            cboTobilloDerDolor.SelectedValue = "2";//  = "N009-OTM00000200";
            cboMunecaIzqDolor.SelectedValue = "2";//  = "N009-OTM00000201";
            cboRodillaDerDolor.SelectedValue = "2";//  = "N009-OTM00000202";
            cboHombroDerAduccion.SelectedValue = "2";//  = "N009-OTM00000203";
            cboCaderaDerDolor.SelectedValue = "2";//  = "N009-OTM00000204";
            cboCodoIzqDolor.SelectedValue = "2";//  = "N009-OTM00000205";
            cboMunecaDerDolor.SelectedValue = "2";//  = "N009-OTM00000206";
            cboHombroDerRotExt.SelectedValue = "2";//  = "N009-OTM00000207";
            cboCodoDerDolor.SelectedValue = "2";//  = "N009-OTM00000208";
            cboHombroIzqDolor.SelectedValue = "2";//  = "N009-OTM00000209";
            cboHombroDerDolor.SelectedValue = "2";//  = "N009-OTM00000210";
            cboCodoIzqSupinacion.SelectedValue = "2";//  = "N009-OTM00000211";
            cboCodoIzqPronacion.SelectedValue = "2";//  = "N009-OTM00000212";
            cboCodoDerSupinacion.SelectedValue = "2";//  = "N009-OTM00000213";
            cboCodoDerPronacion.SelectedValue = "2";//  = "N009-OTM00000214";
            cboMunecaIzqRadial.SelectedValue = "2";//  = "N009-OTM00000215";
            cboMunecaIzqCubital.SelectedValue = "2";//  = "N009-OTM00000216";
            cboMunecaDerRadial.SelectedValue = "2";//  = "N009-OTM00000217";
            cboMunecaDerCubital.SelectedValue = "2";//  = "N009-OTM00000218";
            cboMunecaIzqInversion.SelectedValue = "2";//  = "N009-OTM00000219";
            cboMunecaIzqEversion.SelectedValue = "2";//  = "N009-OTM00000220";
            cboMunecaDerInversion.SelectedValue = "2";//  = "N009-OTM00000221";
            cboMunecaDerEversion.SelectedValue = "2";//  = "N009-OTM00000222";
            cboConclusiones.SelectedValue = "2";//  = "N009-OTM00000224"; 
            #endregion

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
                    if (ctrl.Name.Contains("N009-OTM"))
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
            SaveValueControlForInterfacingESO(senderCtrl.Name, senderCtrl.SelectedValue.ToString());
            _isChangueValueControl = true;
        }

        private void lbl_Leave(object sender, EventArgs e)
        {
            TextBox senderCtrl = (TextBox)sender;
            SaveValueControlForInterfacingESO(senderCtrl.Name, senderCtrl.Text);
            _isChangueValueControl = true;
        }

        private void SaveValueControlForInterfacingESO(string name, string value)
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

        private void calcularTotal1()
        {
            int p1 = txtAbdomenPuntos.Text == "" ? 0 : int.Parse(txtAbdomenPuntos.Text.ToString());
            int p2 = txtCaderaPuntos.Text == "" ? 0 : int.Parse(txtCaderaPuntos.Text.ToString());
            int p3 = txtMusloPuntos.Text == "" ? 0 : int.Parse(txtMusloPuntos.Text.ToString());
            int p4 = txtAbdomenLateralPuntos.Text == "" ? 0 : int.Parse(txtAbdomenLateralPuntos.Text.ToString());
            txtTotalAptitudEspalda.Text = (p1 + p2 + p3 + p4).ToString();

        }

        private void calcularTotal2()
        {
            int p1 = txtAbduccion180Puntos.Text == "" ? 0 : int.Parse(txtAbduccion180Puntos.Text.ToString());
            int p2 = txtAbduccion60Puntos.Text == "" ? 0 : int.Parse(txtAbduccion60Puntos.Text.ToString());
            int p3 = txtRotacion090Puntos.Text == "" ? 0 : int.Parse(txtRotacion090Puntos.Text.ToString());
            int p4 = txtRotacionExtIntPuntos.Text == "" ? 0 : int.Parse(txtRotacionExtIntPuntos.Text.ToString());
            txtTotalRangos.Text = (p1 + p2 + p3 + p4).ToString();

        }

        private void rbAbdomenExcelente_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbdomenExcelente.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000274", "1");
                txtAbdomenPuntos.Text = "1";
                calcularTotal1();
            }
        }
        private void rbAbdomenPromedio_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbdomenPromedio.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000274", "2");
                txtAbdomenPuntos.Text = "2";
                calcularTotal1();
            }
        }
        private void rbAbdomenRegular_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbdomenRegular.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000274", "3");
                txtAbdomenPuntos.Text = "3";
                calcularTotal1();
            }
        }
        private void rbAbdomenPobre_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbdomenPobre.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000274", "4");
                txtAbdomenPuntos.Text = "4";
                calcularTotal1();
            }
        }

        private void rbCaderaExcelente_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCaderaExcelente.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000230", "1");
                txtCaderaPuntos.Text = "1";
                calcularTotal1();
            }
        }
        private void rbCaderaPromedio_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCaderaPromedio.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000230", "2");
                txtCaderaPuntos.Text = "2";
                calcularTotal1();
            }
        }
        private void rbCaderaRegular_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCaderaRegular.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000230", "3");
                txtCaderaPuntos.Text = "3";
                calcularTotal1();
            }
        }
        private void rbCaderaPobre_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCaderaPobre.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000230", "4");
                txtCaderaPuntos.Text = "4";
                calcularTotal1();
            }
        }

        private void rbMusloExcelente_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMusloExcelente.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000236", "1");
                txtMusloPuntos.Text = "1";
                calcularTotal1();
            }
        }
        private void rbMusloPromedio_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMusloPromedio.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000236", "2");
                txtMusloPuntos.Text = "2";
                calcularTotal1();
            }
        }
        private void rbMusloRegular_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMusloRegular.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000236", "3");
                txtMusloPuntos.Text = "3";
                calcularTotal1();
            }
        }
        private void rbMusloPobre_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMusloPobre.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000236", "4");
                txtMusloPuntos.Text = "4";
                calcularTotal1();
            }
        }

        private void rbAbdomenLateralExcelente_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbdomenLateralExcelente.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000242", "1");
                txtAbdomenLateralPuntos.Text = "1";
                calcularTotal1();
            }
        }
        private void rbAbdomenLateralPromedio_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbdomenLateralPromedio.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000242", "2");
                txtAbdomenLateralPuntos.Text = "2";
                calcularTotal1();
            }
        }
        private void rbAbdomenLateralRegular_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbdomenLateralRegular.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000242", "3");
                txtAbdomenLateralPuntos.Text = "3";
                calcularTotal1();
            }
        }
        private void rbAbdomenLateralPobre_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbdomenLateralPobre.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000242", "4");
                txtAbdomenLateralPuntos.Text = "4";
                calcularTotal1();
            }
        }

        private void rbAbduccion180Optimo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbduccion180Optimo.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000248", "1");
                txtAbduccion180Puntos.Text = "1";
                calcularTotal2();
            }
        }
        private void rbAbduccion180Limitado_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbduccion180Limitado.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000248", "2");
                txtAbduccion180Puntos.Text = "2";
                calcularTotal2();
            }
        }
        private void rbAbduccion180MuyLimitado_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbduccion180MuyLimitado.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000248", "3");
                txtAbduccion180Puntos.Text = "3";
                calcularTotal2();
            }
        }

        private void rbAbduccion60Optimo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbduccion60Optimo.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000254", "1");
                txtAbduccion60Puntos.Text = "1";
                calcularTotal2();
            }
        }
        private void rbAbduccion60Limitado_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbduccion60Limitado.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000254", "2");
                txtAbduccion60Puntos.Text = "2";
                calcularTotal2();
            }
        }
        private void rbAbduccion60MuyLimitado_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbduccion60MuyLimitado.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000254", "3");
                txtAbduccion60Puntos.Text = "3";
                calcularTotal2();
            }
        }

        private void rbRotacion090Optimo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRotacion090Optimo.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000260", "1");
                txtRotacion090Puntos.Text = "1";
                calcularTotal2();
            }
        }
        private void rbRotacion090Limitado_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRotacion090Limitado.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000260", "2");
                txtRotacion090Puntos.Text = "2";
                calcularTotal2();
            }
        }
        private void rbRotacion090MuyLimitado_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRotacion090MuyLimitado.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000260", "3");
                txtRotacion090Puntos.Text = "3";
                calcularTotal2();
            }
        }

        private void rbRotacionExtIntOptimo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRotacionExtIntOptimo.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000266", "1");
                txtRotacionExtIntPuntos.Text = "1";
                calcularTotal2();
            }
        }
        private void rbRotacionExtIntLimitado_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRotacionExtIntLimitado.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000266", "2");
                txtRotacionExtIntPuntos.Text = "2";
                calcularTotal2();
            }
        }
        private void rbRotacionExtIntMuyLimitado_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRotacionExtIntMuyLimitado.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000266", "3");
                txtRotacionExtIntPuntos.Text = "3";
                calcularTotal2();
            }
        }

        private void rbAbduccion180DolorSI_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbduccion180DolorSI.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000252", "1");
            }
        }
        private void rbAbduccion180DolorNO_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbduccion180DolorNO.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000252", "2");

            }
        }

        private void rbAbduccion60DolorSI_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbduccion60DolorSI.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000258", "1");
            }
        }
        private void rbAbduccion60DolorNO_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAbduccion60DolorNO.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000258", "2");
            }
        }

        private void rbRotacion090DolorSI_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRotacion090DolorSI.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000264", "1");
            }
        }
        private void rbRotacion090DolorNO_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRotacion090DolorNO.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000264", "2");
            }
        }
        
        private void rbRotacionExtIntDolorSI_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRotacionExtIntDolorSI.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000270", "1");
            }
        }
        private void rbRotacionExtIntDolorNO_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRotacionExtIntDolorNO.Checked)
            {
                SaveValueControlForInterfacingESO("N009-OTM00000270", "2");
            }
        }


    }
}
