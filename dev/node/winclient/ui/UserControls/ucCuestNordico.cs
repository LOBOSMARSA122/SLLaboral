using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;

namespace Sigesoft.Node.WinClient.UI.UserControls
{
    public partial class UcCuestNordico : UserControl
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

        public UcCuestNordico()
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

        private void UcCuestNordico_Load(object sender, EventArgs e)
        {
            cbo1Cuello.Name = "N009-CSN00000001";
            cbo1Hombro.Name = "N009-CSN00000002";
            cbo1Dorsal.Name = "N009-CSN00000003";
            cbo1Codo.Name = "N009-CSN00000004";
            cbo1Mano.Name = "N009-CSN00000005";

            cbo1HombroDir.Name = "N009-CSN00000006";
            cbo1CodoDir.Name = "N009-CSN00000007";
            cbo1ManoDir.Name = "N009-CSN00000008";

            txt2Cuello.Name = "N009-CSN00000009";
            txt2Hombro.Name = "N009-CSN00000010";
            txt2Dorsal.Name = "N009-CSN0000001";
            txt2Codo.Name = "N009-CSN00000012";
            txt2Mano.Name = "N009-CSN00000013";

            cbo3Cuello.Name = "N009-CSN00000014";
            cbo3Hombro.Name = "N009-CSN00000015";
            cbo3Dorsal.Name = "N009-CSN00000016";
            cbo3Codo.Name = "N009-CSN00000017";
            cbo3Mano.Name = "N009-CSN00000018";

            cbo4Cuello.Name = "N009-CSN00000019";
            cbo4Hombro.Name = "N009-CSN00000020";
            cbo4Dorsal.Name = "N009-CSN00000021";
            cbo4Codo.Name = "N009-CSN00000022";
            cbo4Mano.Name = "N009-CSN00000023";

            cbo5Cuello.Name = "N009-CSN00000024";
            cbo5Hombro.Name = "N009-CSN00000025";
            cbo5Dorsal.Name = "N009-CSN00000026";
            cbo5Codo.Name = "N009-CSN00000027";
            cbo5Mano.Name = "N009-CSN00000028";

            cbo6Cuello.Name = "N009-CSN00000029";
            cbo6Hombro.Name = "N009-CSN00000030";
            cbo6Dorsal.Name = "N009-CSN00000031";
            cbo6Codo.Name = "N009-CSN00000032";
            cbo6Mano.Name = "N009-CSN00000033";

            cbo7Cuello.Name = "N009-CSN00000034";
            cbo7Hombro.Name = "N009-CSN00000035";
            cbo7Dorsal.Name = "N009-CSN00000036";
            cbo7Codo.Name = "N009-CSN00000037";
            cbo7Mano.Name = "N009-CSN00000038";

            cbo8Cuello.Name = "N009-CSN00000039";
            cbo8Hombro.Name = "N009-CSN00000040";
            cbo8Dorsal.Name = "N009-CSN00000041";
            cbo8Codo.Name = "N009-CSN00000042";
            cbo8Mano.Name = "N009-CSN00000043";

            cbo9Cuello.Name = "N009-CSN00000044";
            cbo9Hombro.Name = "N009-CSN00000045";
            cbo9Dorsal.Name = "N009-CSN00000046";
            cbo9Codo.Name = "N009-CSN00000047";
            cbo9Mano.Name = "N009-CSN00000048";

            cbo10Cuello.Name = "N009-CSN00000049";
            cbo10Hombro.Name = "N009-CSN00000050";
            cbo10Dorsal.Name = "N009-CSN00000051";
            cbo10Codo.Name = "N009-CSN00000052";
            cbo10Mano.Name = "N009-CSN00000053";

            txt11Cuello.Name = "N009-CSN00000054";
            txt11Hombro.Name = "N009-CSN00000055";
            txt11Dorsal.Name = "N009-CSN0000056";
            txt11Codo.Name = "N009-CSN00000057";
            txt11Mano.Name = "N009-CSN00000058";

            #region BindCombos
            OperationResult objOperationResult = new OperationResult();
            SystemParameterBL objSystemParameterBl = new SystemParameterBL();
            
            Utils.LoadDropDownList(cbo1Cuello, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo1Hombro, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo1Dorsal, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo1Codo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo1Mano, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo1HombroDir, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 246), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo1CodoDir, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 246), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo1ManoDir, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 246), DropDownListAction.Select);

            Utils.LoadDropDownList(cbo3Cuello, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo3Hombro, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo3Dorsal, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo3Codo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo3Mano, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            
            Utils.LoadDropDownList(cbo4Cuello, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo4Hombro, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo4Dorsal, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo4Codo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo4Mano, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);

            Utils.LoadDropDownList(cbo5Cuello, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 243), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo5Hombro, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 243), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo5Dorsal, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 243), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo5Codo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 243), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo5Mano, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 243), DropDownListAction.Select);

            Utils.LoadDropDownList(cbo6Cuello, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 244), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo6Hombro, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 244), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo6Dorsal, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 244), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo6Codo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 244), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo6Mano, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 244), DropDownListAction.Select);

            Utils.LoadDropDownList(cbo7Cuello, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 244), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo7Hombro, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 244), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo7Dorsal, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 244), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo7Codo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 244), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo7Mano, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 244), DropDownListAction.Select);

            Utils.LoadDropDownList(cbo8Cuello, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo8Hombro, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo8Dorsal, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo8Codo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo8Mano, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);

            Utils.LoadDropDownList(cbo9Cuello, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo9Hombro, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo9Dorsal, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo9Codo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo9Mano, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 111), DropDownListAction.Select);

            Utils.LoadDropDownList(cbo10Cuello, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 245), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo10Hombro, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 245), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo10Dorsal, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 245), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo10Codo, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 245), DropDownListAction.Select);
            Utils.LoadDropDownList(cbo10Mano, "Value1", "Id", objSystemParameterBl.GetSystemParameterForCombo(ref objOperationResult, 245), DropDownListAction.Select);


            cbo1Cuello.SelectedValue = "0";
            cbo1Hombro.SelectedValue = "0";
            cbo1Dorsal.SelectedValue = "0";
            cbo1Codo.SelectedValue = "0";
            cbo1Mano.SelectedValue = "0";

            cbo1HombroDir.SelectedValue = "3"; ;
            cbo1CodoDir.SelectedValue = "3";
            cbo1ManoDir.SelectedValue = "3";


            SearchControlAndSetEvents(this);

            #endregion
        }

        private void SearchControlAndSetEvents(Control ctrlContainer)
        {
            foreach (Control ctrl in ctrlContainer.Controls)
            {
                if (ctrl is TextBox)
                {
                    if (ctrl.Name.Contains("N009-CSN"))
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
            _userControlValores.v_ComponentId = Constants.CUESTIONARIO_NORDICO;

            _listOfAtencionAdulto1.Add(_userControlValores);

            DataSource = _listOfAtencionAdulto1;

            #endregion
        }

    }
}
