bindsOrderAddFormEvent = function () {
    $("#DeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300,
        value: defaultDeptID,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#DeptID').combotree('clear');
                $('#sProfessionID').combobox('clear');
                $('#sProfessionID').combobox('loadData', '');
                $('#Scheme').combobox('clear');
                $('#Scheme').combobox('loadData', '');
                $('#Semester').combobox('clear');
                $('#Semester').combobox('loadData', '');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                var year = $("#Year").combobox("getValue");
                var month = $("#Month").combobox("getValue");
                if (year != "" && month != "") {
                    loadMajor(node.id, year, month);
                }
            }
        }
    });

    $("#Year").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=No&Is=No",
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            var dept = $("#DeptID").combotree("getValue");
            var month = $("#Month").combobox("getValue");
            if (dept != "" && month != "") {
                loadMajor(dept, data.Id, month);
            }
        }
    });

    $("#Month").combobox({
        url: "../Refe/SelList?RefeTypeID=18",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            var dept = $("#DeptID").combotree("getValue");
            var year = $("#Year").combobox("getValue");
            if (dept != "" && year != "") {
                loadMajor(dept, year, data.Value);
            }
        }
    });

    $("#btnAddSetsOrder").click(function () {
        Easy.OpenDialogEvent("#choosesOrderDetail", "设置缴费项", 630, 400, "../sOrder/SetsOrder", "#choosesOrderDetail-buttons");
    });

    $("#btnSavesOrderDetail").click(function () {
        $("#itemDetailGrid").edatagrid('saveRow');
        var data = $('#itemDetailGrid').datagrid("getData");
        if (data.rows.length > 0) {
            var sName = "";
            var tips = "";
            for (var i = 0; i < data.rows.length; i++) {
                sName += data.rows[i].DetailName + ",";
                if (data.rows[i].Sort == "") {
                    tips = "请选择" + data.rows[i].DetailName + "的项目分类";
                    break;
                }
                if (data.rows[i].IsGive == "") {
                    tips = "请选择" + data.rows[i].DetailName + "的类别";
                    break;
                }
                if (data.rows[i].ShouldMoney.toString() == "") {
                    tips = data.rows[i].DetailName + "的应收金额不能为空";
                    break;
                }
                if (parseFloat(data.rows[i].ShouldMoney) == 0) {
                    tips = data.rows[i].DetailName + "的应收金额不能为0";
                    break;
                }
                if (data.rows[i].LimitTime == "") {
                    tips = "请选择" + data.rows[i].DetailName + "的缴费截止时间";
                    break;
                }
            }
            if (tips != "") {
                Easy.centerShow("系统消息", tips, 3000);
                return false;
            }
            else {
                $("#sOrderContent").textbox("setValue", JSON.stringify(data));
                $("#sOrderName").textbox("setValue", sName);
                $("#choosesOrderDetail").dialog('close');
            }
        }
        else {
            Easy.centerShow("系统消息", "请添加收费项目", 1000);
        }
    });
}

loadMajor = function (dept, year, month) {
    $('#Scheme').combobox('clear');
    $('#Scheme').combobox('loadData', '');
    $('#Semester').combobox('clear');
    $('#Semester').combobox('loadData', '');
    $("#sProfessionID").combobox({
        url: "../sEnroll/SelMajor",
        queryParams: { DeptID: dept, Year: year, Month: month },
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: true,
        editable: false,
        onSelect: function (data1) {
            var ids = $("#sProfessionID").combobox("getValues");
            loadScheme(ids.toString());
        },
        onUnselect: function (data2) {
            var un_ids = $("#sProfessionID").combobox("getValues");
            if (un_ids == "") {
                un_ids = "0";
            }
            loadScheme(un_ids.toString());
        }
    });
}

loadScheme = function (majorIDs) {
    $('#Semester').combobox('clear');
    $('#Semester').combobox('loadData', '');
    $("#Scheme").combobox({
        url: "../sOrderAdd/SelScheme",
        queryParams: { MajorID: majorIDs },
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            loadSemester(data.Id);
        }
    });
}

loadSemester = function (schemeID) {
    $("#Semester").combobox({
        url: "../sOrderAdd/SelSemester",
        queryParams: { SchemeID: schemeID },
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
}

bindsOrderAddFormEvent();