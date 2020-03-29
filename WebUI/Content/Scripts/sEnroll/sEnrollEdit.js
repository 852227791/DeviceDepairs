bindFormEvent = function () {
    $("#repeatSignEdit").hide();

    if (!Easy.bindPowerValidationEventUsetoHidePower(menuId, "1", "goon")) {
        $("#editGoOnSign").attr("disabled", "disabled");
    }
    else {
        $("#editGoOnSign").attr("disabled", false);
    }

    if (!Easy.bindPowerValidationEventUsetoHidePower(menuId, "1", "repeat")) {
        $("#editRepeatSign").attr("disabled", "disabled");
    }
    else {
        $("#editRepeatSign").attr("disabled", false);
    }

    if (defaultDeptID!="") {
        $("#editDeptAreaId").combobox({
            url: "../DeptArea/GetDeptAreaCombobox",
            queryParams: { DeptID: defaultDeptID },
            valueField: "id",
            textField: "name",
            panelHeight: 120,
            multiple: false,
            editable: false
        });
    }
    $("#editDeptID").combotree({
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
                $('#editDeptID').combotree('clear');
                $('#editsProfessionID').combobox('clear');
                $('#editsProfessionID').combobox('loadData', '');
                $('#editScheme').textbox('setValue', '');
                $('#editSchemeName').textbox('setValue', '');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                var year = $("#editYear").combobox("getValue");
                var month = $("#editMonth").combobox("getValue");
                if (year != "" && month != "") {
                    loadMajor(node.id, year, month);
                    $('#editScheme').textbox('setValue', '');
                    $('#editSchemeName').textbox('setValue', '');
                }

                $("#editDeptAreaId").combobox({
                    url: "../DeptArea/GetDeptAreaCombobox",
                    queryParams: { DeptID: node.id },
                    valueField: "id",
                    textField: "name",
                    panelHeight: 120,
                    multiple: false,
                    editable: false
                });
            }
        }
    });

    $("#btnChooseStudent").click(function () {
        Easy.OpenDialogEvent("#chooseStudent", "选择学生", 600, 400, "../Student/ChooseStudent", "#chooseStudent-buttons");
    });

    $("#btnSaveStudent").click(function () {
        var rows = $("#choosestudentGrid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "学生")) {
            $("#editStudentID").textbox("setValue", rows[0].StudentID);
            $("#editStuName").textbox("setValue", rows[0].Name + "_" + rows[0].IDCard);
            $("#chooseStudent").dialog('close');
            var result = Easy.bindSelectInfo("../sEnroll/CheckStudentIsEnroll", rows[0].StudentID);
            if (result.Data) {
                //已生成过学号
                $("#repeatSignEdit").show();
            }
            else {
                $("#repeatSignEdit").hide();
            }
            //重新选人就回到默认
            document.getElementById("editDefaultSign").checked = true;
        }
    });

    $("#editYear").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=No&Is=No",
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            var dept = $("#editDeptID").combotree("getValue");
            var month = $("#editMonth").combobox("getValue");
            if (dept != "" && month != "") {
                loadMajor(dept, data.Id, month);
                $('#editScheme').textbox('setValue', '');
                $('#editSchemeName').textbox('setValue', '');
            }
        }
    });

    $("#editMonth").combobox({
        url: "../Refe/SelList?RefeTypeID=18",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            var dept = $("#editDeptID").combotree("getValue");
            var year = $("#editYear").combobox("getValue");
            if (dept != "" && year != "") {
                loadMajor(dept, year, data.Value);
                $('#editScheme').textbox('setValue', '');
                $('#editSchemeName').textbox('setValue', '');
            }
        }
    });

    $("#editEnrollLevel").combobox({
        url: "../Refe/SelList?RefeTypeID=17",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });

    $("#editEnrollTime").datebox({
        value: Easy.GetDateTimeNow(),
        editable: false
    });

    $("#editsProfessionID").combobox({ editable: false });

    $("#btnChooseScheme").click(function () {
        var major = $("#editsProfessionID").combotree("getValue");
        var year = $("#editYear").combobox("getValue");
        var month = $("#editMonth").combobox("getValue");
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
        sEnrollMajor = $("#editsProfessionID").combotree("getValue");
        sEnrollYear = $("#editYear").combobox("getValue");
        sEnrollMonth = $("#editMonth").combobox("getValue");
        sEnrollSchemeTempData = $("#editScheme").textbox("getValue");
        Easy.OpenDialogEvent("#choosesFeeScheme", "选择缴费方案", 800, 600, "../sEnroll/ChooseSchemeList", "#choosesFeeScheme-buttons");
    });

    $("#btnSavesFeeScheme").click(function () {
        var data = $('#chooseschemegrid2').datagrid("getData");
        if (data.rows.length > 0) {
            var sName = "";
            for (var i = 0; i < data.rows.length; i++) {
                sName += data.rows[i].Name + ",";
            }
            $("#editScheme").textbox("setValue", JSON.stringify(data));
            $("#editSchemeName").textbox("setValue", sName.substr(0, sName.length - 1));
        }
        else {
            $("#editScheme").textbox("setValue", "");
            $("#editSchemeName").textbox("setValue", "");
        }
        $("#choosesFeeScheme").dialog('close');
    });
}

loadMajor = function (dept, year, month) {
    $("#editsProfessionID").combobox({
        url: "../sEnroll/SelMajor",
        queryParams: { DeptID: dept, Year: year, Month: month },
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (record) {
            $('#editScheme').textbox('setValue', '');
            $('#editSchemeName').textbox('setValue', '');
            loadClassCombobox(record.Id);
        }
    });
}

loadClassCombobox = function (sProfessionId) {
    $("#editClassID").combobox({
        url: "../Class/ClassComboboxBysProfessionID",
        queryParams: { sProfessionID: sProfessionId },
        valueField: "Value",
        textField: "Text",
        multiple: false
    });
}

loadEditShceme = function (editsEnrollsProfessionID) {
    var result = Easy.bindSelectInfo("../sEnroll/GetEditShceme", sEnrollsProfessionID);
    $("#editScheme").textbox("setValue", result.Message);
}

bindSelectInfo = function (sEnrollsProfessionID) {
    if (sEnrollsProfessionID != "0") {
        var result = Easy.bindSelectInfo("../sEnroll/CheckIsGiveFee", sEnrollsProfessionID);
        if (result.Data) {
            $("#editsEnrollsProfessionID").textbox({ readonly: true });
            $("#editStudentID").textbox({ readonly: true });
            $("#editDeptID").combotree({ readonly: true });
            $("#editStuName").textbox({ readonly: true });
            $("#editYear").combobox({ readonly: true });
            $("#editMonth").combobox({ readonly: true });
            $("#editEnrollLevel").combobox({ readonly: true });
            $("#editEnrollTime").datebox({ readonly: true });
            $("#editSchemeName").textbox({ readonly: true });
            $("#editsProfessionID").combobox({ readonly: true });
        }

        $("#btnChooseStudent").hide();
        setTimeout(function () {
            var result1 = Easy.bindSelectInfo("../sEnroll/SelectsEnroll", sEnrollsProfessionID);
            var enroll = JSON.parse(result1.Message)[0];
            $("#editsEnrollsProfessionID").textbox("setValue", enroll.sEnrollsProfessionID);
            $("#editStudentID").textbox("setValue", enroll.StudentID);
            $("#editDeptID").combotree("setValue", enroll.DeptID);
            $("#editStuName").textbox("setValue", enroll.StuName);
            $("#editYear").combobox("setValue", enroll.Year);
            $("#editMonth").combobox("setValue", enroll.Month);
            $("#editEnrollLevel").combobox("setValue", enroll.EnrollLevel);
            loadMajor(enroll.DeptID, enroll.Year, enroll.Month);
            $("#editsProfessionID").combobox("setValue", enroll.sProfessionID);
            loadClassCombobox(enroll.sProfessionID);
            $("#editClassID").combobox("setValue", enroll.ClassID);
            $("#editEnrollTime").datebox("setValue", enroll.EnrollTime);
            $("#editSchemeName").textbox("setValue", enroll.SchemeName.substr(0, enroll.SchemeName.length - 1));
            $("#editRemark").textbox("setValue", enroll.Remark.replace(/<br \/>/g, "\r\n"));
            loadEditShceme(enroll.editsEnrollsProfessionID);
            $("#editDeptAreaId").combobox({
                url: "../DeptArea/GetDeptAreaCombobox",
                queryParams: { DeptID: enroll.DeptID },
                valueField: "id",
                textField: "name",
                panelHeight: 120,
                multiple: false,
                editable: false,
                value: enroll.DeptAreaID
            });
            var result = Easy.bindSelectInfo("../sEnroll/CheckIsShouldMoney", sEnrollsProfessionID);
            if (result.Data) {
                $("#btnChooseScheme").hide();
            }
            else {
                $("#btnChooseScheme").show();
            }
        }, 1);
    }
    else {
        $("#btnChooseStudent").show();
        $("#btnChooseScheme").show();
    }
}

bindFormEvent();

bindSelectInfo(sEnrollsProfessionID);
