chooseSchame = "2";
bindMajorFormEvent = function () {
    var row = $("#grid").datagrid("getSelected");

    setTimeout(function () {
        $("#MajorsEnrollID").textbox("setValue", row.sEnrollID);
        $("#MajorsEnrollsProfessionID").textbox("setValue", row.sEnrollsProfessionID);
        //$("#MajorEnrollTime").datebox("setValue", row.SignTime);
    }, 1);

    $("#MajorDeptID").combotree({
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
                $('#MajorDeptID').combotree('clear');
                $('#MajorsProfessionID').combobox('clear');
                $('#MajorsProfessionID').combobox('loadData', '');
                $('#MajorScheme').textbox('setValue', '');
                $('#MajorSchemeName').textbox('setValue', '');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                var year = $("#MajorYear").combobox("getValue");
                var month = $("#MajorMonth").combobox("getValue");
                if (year != "" && month != "") {
                    loadTurnMajor(node.id, year, month);
                    $('#MajorScheme').textbox('setValue', '');
                    $('#MajorSchemeName').textbox('setValue', '');
                }
            }
        }
    });

    $("#MajorYear").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=No&Is=No",
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            var dept = $("#MajorDeptID").combotree("getValue");
            var month = $("#MajorMonth").combobox("getValue");
            if (dept != "" && month != "") {
                loadTurnMajor(dept, data.Id, month);
                $('#MajorScheme').textbox('setValue', '');
                $('#MajorSchemeName').textbox('setValue', '');
            }
        }
    });

    $("#MajorMonth").combobox({
        url: "../Refe/SelList?RefeTypeID=18",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            var dept = $("#MajorDeptID").combotree("getValue");
            var year = $("#MajorYear").combobox("getValue");
            if (dept != "" && year != "") {
                loadTurnMajor(dept, year, data.Value);
                $('#MajorScheme').textbox('setValue', '');
                $('#MajorSchemeName').textbox('setValue', '');
            }
        }
    });

    $("#MajorEnrollLevel").combobox({
        url: "../Refe/SelList?RefeTypeID=17",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });
    $("#Read").combobox({
        url: "../sEnroll/GetChangeMajorYearCombobox",
        queryParams: { sEnrollsProfessionID: row.sEnrollsProfessionID },
        valueField: "id",
        textField: "text",
        panelHeight: 120,
        multiple: true,
        editable: false
    });
    $("#MajorChangeTime").datebox({
        value: Easy.GetDateTimeNow(),
        editable: false,
        readonly: true
    });

    //$("#MajorEnrollTime").datebox({
    //    editable: false
    //});

    $("#MajorsProfessionID").combobox({ editable: false });

    $("#btnChoosesFeeScheme").click(function () {
        var major = $("#MajorsProfessionID").combotree("getValue");
        var year = $("#MajorYear").combobox("getValue");
        var month = $("#MajorMonth").combobox("getValue");
        var level = $("#MajorEnrollLevel").combobox("getValue")
        if (year == "") {
            Easy.centerShow("系统消息", "请选择年份", 3000);
            return false;
        }
        if (month == "") {
            Easy.centerShow("系统消息", "请选择月份", 3000);
            return false;
        }
        if (major == "") {
            Easy.centerShow("系统消息", "请选择专业", 3000);
            return false;
        }
        if (level === "") {
            Easy.centerShow("系统消息", "请选择学习层次", 3000);
            return false;
        }
        enrollLevel = level;
        sEnrollMajor = $("#MajorsProfessionID").combotree("getValue");
        sEnrollYear = $("#MajorYear").combobox("getValue");
        sEnrollMonth = $("#MajorMonth").combobox("getValue");
        sEnrollSchemeTempData = $("#MajorScheme").textbox("getValue");
        Easy.OpenDialogEvent("#choosesFeeScheme", "选择缴费方案", 800, 600, "../sEnroll/ChooseSchemeList", "#choosesFeeScheme-buttons");
    });

    $("#btnSavesFeeScheme").click(function () {
        var data = $('#chooseschemegrid2').datagrid("getData");
        var enrollLevel = $("#MajorEnrollLevel").combobox("getValue");
        var flag = true;
        if (enrollLevel == "1" || enrollLevel == "2" || enrollLevel == "3") {
            if (data.rows.length != 1) {
                flag = false;
            }
        }
        else if (enrollLevel == "4" || enrollLevel == "5") {
            if (data.rows.length != 2) {
                flag = false;
            }
        }
        else if (enrollLevel == "6") {
            if (data.rows.length != 3) {
                flag = false;
            }
        }
        if (!flag) {
            Easy.centerShow("系统消息", "学习层次和方案数量不匹配", 2000);
            return false;
        }

        if (data.rows.length > 0) {
            var sName = "";
            for (var i = 0; i < data.rows.length; i++) {
                sName += data.rows[i].Name + ",";
            }
            $("#MajorScheme").textbox("setValue", JSON.stringify(data));
            $("#MajorSchemeName").textbox("setValue", sName.substr(0, sName.length - 1));
        }
        else {
            $("#MajorScheme").textbox("setValue", "");
            $("#MajorSchemeName").textbox("setValue", "");
        }
        $("#choosesFeeScheme").dialog('close');
    });
}

loadTurnMajor = function (dept, year, month) {
    $("#MajorsProfessionID").combobox({
        url: "../sEnroll/SelMajor",
        queryParams: { DeptID: dept, Year: year, Month: month },
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (record) {
            $("#MajorScheme").textbox("setValue", "");
            $("#MajorSchemeName").textbox("setValue", "");
            loadTurnClassCombobox(record.Id);
        }
    });
}

loadTurnClassCombobox = function (sProfessionId) {
    $("#MajorClassID").combobox({
        url: "../Class/ClassComboboxBysProfessionID",
        queryParams: { sProfessionID: sProfessionId },
        valueField: "Value",
        textField: "Text",
        multiple: false
    });
}

bindMajorFormEvent();
