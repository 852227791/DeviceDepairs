firstFunction = function () {
    bindSearchFormEvent();

    //加载表格
    initTable({ MenuID: menuId });

    //加载搜索按钮的点击事件
    bindSearchClickEvent();
}

bindSearchFormEvent = function () {
    $("#DeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        value: defaultDeptID,
        panelWidth: 300,
        multiple: true,
        onlyLeafCheck: true
    });
    $("#selYear").combobox({
        url: "../Common/YearCombobox?Year=2010&IsSelected=No&Is=No",
        valueField: "Id",
        textField: "Text",
        panelHeight: 120,
        multiple: true,
        editable: false
    });
    $("#selMonth").combobox({
        url: "../Refe/SelList?RefeTypeID=18",
        valueField: "Value",
        textField: "RefeName",
        multiple: true,
        editable: false
    });
}

//加载表格
initTable = function (queryData) {
    $("#grid").treegrid({
        striped: true,//斑马线
        singleSelect: true,//只允许选择一行
        showFooter: true,
        idField: 'DeptID',
        treeField: 'DeptName',
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "DeptName", title: "收费单位", width: 300, sortable: true, rowspan: 2 },
            { title: "统招交费人数", colspan: 5 },
            { title: "成教交费人数", colspan: 5 },
            { field: "C_BeforeEnrollNum", title: "成教预报名", sortable: true, halign: 'left', align: 'right', rowspan: 2 },
            { field: "ZFeeNumSum", title: "转正报人数", sortable: true, halign: 'left', align: 'right', rowspan: 2 },
            { field: "Z_FeeNumSum", title: "专升本人数", sortable: true, halign: 'left', align: 'right', rowspan: 2 },
            { field: "W_FeeNumSum", title: "五年一贯制人数", sortable: true, halign: 'left', align: 'right', rowspan: 2 },
            { field: "X_FeeNumSum", title: "中小学人数", sortable: true, halign: 'left', align: 'right', rowspan: 2 }
        ], [
            { field: "T_FeeNumSum", title: "总数", sortable: true, halign: 'left', align: 'right' },
            { field: "T_FeeNum1", title: "主校区", sortable: true, halign: 'left', align: 'right' },
            { field: "T_FeeNum2", title: "校区1", sortable: true, halign: 'left', align: 'right' },
            { field: "T_FeeNum3", title: "校区2", sortable: true, halign: 'left', align: 'right' },
            { field: "T_FeeNum4", title: "校区3", sortable: true, halign: 'left', align: 'right' },
            { field: "C_FeeNumSum", title: "总数", sortable: true, halign: 'left', align: 'right' },
            { field: "C_FeeNum1", title: "主校区", sortable: true, halign: 'left', align: 'right' },
            { field: "C_FeeNum2", title: "校区1", sortable: true, halign: 'left', align: 'right' },
            { field: "C_FeeNum3", title: "校区2", sortable: true, halign: 'left', align: 'right' },
            { field: "C_FeeNum4", title: "校区3", sortable: true, halign: 'left', align: 'right' }
        ]],
        toolbar: Easy.loadToolbar(menuId, "1")
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        $("#grid").treegrid({
            queryParams: queryData,//异步查询的参数
            url: "../sReport/GetEnrollList",
        });
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        $("#grid").treegrid({ url: "", data: [] });
    });
}

//导出
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        Easy.DeriveFileToGrid(".datagrid-view2", "学费报名报表.xls");
    }
}

rerurnQueryData = function () {
    var deptId = "" + $("#DeptID").combotree("getValues");
    var data = {//得到用户输入的参数
        MenuID: menuId,
        txtDeptID: deptId,
        selYear: "" + $("#selYear").combobox("getValues"),
        selMonth: "" + $("#selMonth").combobox("getValues"),
        txtTimeS: "" + $("#txtTimeS").datebox("getValue"),
        txtTimeE: "" + $("#txtTimeE").datebox("getValue")
    }
    return data;
}