var isSelected = 0;
firstFunction = function () {
    Easy.bindOpenCloseSearchBoxEvent(90);
    bindSerchForm();
    bindsFeeDetailTable();
    bindSearchClickEvent();
}
bindsFeeDetailTable = function () {
    $("#grid").datagrid({
        striped: true,//斑马线
        showFooter: true,
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: false,//分页
        columns: [[
            { field: "MajorName", title: "专业名称", width: 180, sortable: true },
            { field: "TongZhao", title: "统招缴费人数", sortable: true, align: "right" },
            { field: "ChengJiao", title: "成教缴费人数", sortable: true, align: "right" },
            { field: "ChengJiaoBefore", title: "成教预报名人数", sortable: true, align: "right" },
            { field: "ChengJiaoZhuan", title: "转正报人数", sortable: true, align: "right" },
            { field: "ZhuanShenBen", title: "专升本缴费人数", sortable: true, align: "right" },
            { field: "WuNian", title: "五年一贯制缴费人数", sortable: true, align: "right" },
            { field: "ZhongXiaoXue", title: "中小学缴费人数", sortable: true, align: "right" }
        ]],
        sortName: "EnglishName", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        onLoadSuccess: function (data) {
            var TongZhao = 0;
            var ChengJiao = 0;
            var ChengJiaoBefore = 0;
            var ChengJiaoZhuan = 0;
            var WuNian = 0;
            var ZhuanShenBen = 0;
            var ZhongXiaoXue = 0;
            for (var i = 0; i < data.rows.length; i++) {
                var row = data.rows[i];
                TongZhao += parseFloat(row.TongZhao);
                ChengJiao += parseFloat(row.ChengJiao);
                ChengJiaoBefore += parseFloat(row.ChengJiaoBefore);
                ChengJiaoZhuan += parseFloat(row.ChengJiaoZhuan);
                WuNian += parseFloat(row.WuNian);
                ZhuanShenBen += parseFloat(row.ZhuanShenBen);
                ZhongXiaoXue += parseFloat(row.ZhongXiaoXue);
            }
            $('#grid').datagrid('reloadFooter', [
	            { TongZhao: '合计：' + TongZhao, ChengJiao: '合计：' + ChengJiao, ChengJiaoBefore: '合计：' + ChengJiaoBefore, ChengJiaoZhuan: '合计：' + ChengJiaoZhuan, WuNian: '合计：' + WuNian, ZhuanShenBen: '合计：' + ZhuanShenBen, ZhongXiaoXue: '合计：' + ZhongXiaoXue },
            ]);

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

    $("#selEnrollLevel").combobox({
        url: "../Refe/SelList?RefeTypeID=17",
        valueField: "Value",
        textField: "RefeName",
        multiple: true,
        editable: false
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
download = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "download")) {
        Easy.DeriveFileToGrid(".datagrid-view2", "专业缴费人数报表.xls");
    }
};
queryHandler = function (searchText, event) {
    $('#treeDetail').combotree('tree').tree("search", searchText);
}




bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var data = getSeachData();
        if (data.treeDeptID === "") {
            Easy.centerShow("系统消息", "请选择校区", 2000)
            return false;
        }
        $("#grid").datagrid({ url: "../sReport/GetMajorFeeCount", queryParams: data, pageNumber: 1 });
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        $("#grid").datagrid({ url: "", data:[]});
    });
}
var getSeachData = function () {
    var queueData = {
        MenuID: menuId,
        treeDeptID: $("#treeDeptID").combotree("getValue"),
        txtTimeS: $("#txtTimeS").datebox("getValue"),
        txtTimeE: $("#txtTimeE").datebox("getValue"),
        selEnrollLevel: "" + $("#selEnrollLevel").combobox("getValues"),
        selYear: "" + $("#selYear").combobox("getValues"),
        selMonth: "" + $("#selMonth").combobox("getValues")
    }
    return queueData;
}