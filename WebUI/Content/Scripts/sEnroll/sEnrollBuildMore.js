bindBuldFormEvent = function () {
    $("#buildDeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        value: defaultDeptID,
        panelWidth: 300,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#buildDeptID').combotree('clear');
                $('#buildsProfessionID').combobox('clear');
                $('#buildsProfessionID').combobox('loadData', '');
                $('#buildScheme').combobox('clear');
                $('#buildScheme').combobox('loadData', '');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                var year = $("#buildYear").combobox("getValue");
                var month = $("#buildMonth").combobox("getValue");
                if (year != "" && month != "") {
                    loadMajor(node.id, year, month);
                    loadScheme(node.id, year, month);
                }
            }
        }
    });

    $("#buildYear").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=No&Is=No",
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            var dept = $("#buildDeptID").combotree("getValue");
            var month = $("#buildMonth").combobox("getValue");
            if (dept != "" && month != "") {
                loadMajor(dept, data.Id, month);
                loadScheme(dept, data.Id, month);
            }
        }
    });

    $("#buildMonth").combobox({
        url: "../Refe/SelList?RefeTypeID=18",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false,
        onSelect: function (data) {
            var dept = $("#buildDeptID").combotree("getValue");
            var year = $("#buildYear").combobox("getValue");
            if (dept != "" && year != "") {
                loadMajor(dept, year, data.Value);
                loadScheme(dept, year, data.Value);
            }
        }
    });

    $("#buildEnrollLevel").combobox({
        url: "../Refe/SelList?RefeTypeID=17",
        valueField: "Value",
        textField: "RefeName",
        panelHeight: 120,
        multiple: false,
        editable: false
    });

    $("#buildsProfessionID").combobox({ editable: false });

    $("#buildScheme").combobox({ editable: false });
}

loadMajor = function (dept, year, month) {
    $("#buildsProfessionID").combobox({
        url: "../sEnroll/SelMajor",
        queryParams: { DeptID: dept, Year: year, Month: month },
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: true,
        editable: false
    });
}

loadScheme = function (dept, year, month) {
    $("#buildScheme").combobox({
        url: "../sEnroll/SelScheme",
        queryParams: { DeptID: dept, Year: year, Month: month },
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: true,
        editable: false
    });
}

bindBuldFormEvent();