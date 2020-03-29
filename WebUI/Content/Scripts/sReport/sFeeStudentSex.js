var isSelected = 0;
firstFunction = function () {
    bindSerchForm();
    bindsFeeDetailTable();
    bindSearchClickEvent();
}
bindsFeeDetailTable = function () {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: false,//分页
        showFooter: true,
        pageSize: 20,
        columns: [[
          { field: "DeptName", title: "收费单位", width: 300, sortable: true, rowspan: 2 },
          { title: "统招", sortable: true, colspan: 2 },
          { title: "成教", sortable: true, colspan: 2 },
          { title: "成教预报", sortable: true, colspan: 2 },
          { title: "转正报", sortable: true, colspan: 2 },
          { title: "专升本", sortable: true, colspan: 2 },
          { title: "五年一贯制", sortable: true, colspan: 2 },
          { title: "中小学", sortable: true, colspan: 2 }
        ], [
            { field: "TongZhaoNan", title: "男", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "TongZhaoNv", title: "女", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "ChengJiaoNan", title: "男", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "ChengJiaoNv", title: "女", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "ChengJiaoBeforeNan", title: "男", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "ChengJiaoBeforeNv", title: "女", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "ChengJiaoZhuanNan", title: "男", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "ChengJiaoZhuanNv", title: "女", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "ZhuanShenBenNan", title: "男", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "ZhuanShenBenNv", title: "女", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "WuNianNan", title: "男", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "WuNianNv", title: "女", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "ZhongXiaoXueNan", title: "男", sortable: true, halign: 'center', align: 'right', width: 80 },
            { field: "ZhongXiaoXueNv", title: "女", sortable: true, halign: 'center', align: 'right', width: 80 }
        ]],
        sortName: "FeeUser", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        onLoadSuccess: function (data) {
            $('#grid').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
        }
    });
}

bindSerchForm = function () {
    $("#treeDeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
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

queryHandler = function (searchText, event) {
    $('#treeDetail').combotree('tree').tree("search", searchText);
}

download = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "download")) {
        Easy.DeriveFileToGrid(".datagrid-view2", "收费员收费报表.xls");
    }
};
bindSelect = function (data) {
    $("#grid").datagrid({ url: "../sReport/GetFeeStudentSexList", queryParams: data, pageNumber: 1 });
}
bindSearchClickEvent = function () {

    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var data = getSeachData();
        if (data.deptId === "") {
            Easy.centerShow("系统消息", "请选择校区", 2000);
            return false;
        }
        bindSelect(data);
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        $("#grid").datagrid({ url: "" });
    });
}
var getSeachData = function () {
    var queueData = {
        MenuID: menuId,
        deptId: $("#treeDeptID").combotree("getValue"),
        selYear: "" + $("#selYear").combobox("getValues"),
        selMonth: "" + $("#selMonth").combobox("getValues")
    }
    return queueData;
}