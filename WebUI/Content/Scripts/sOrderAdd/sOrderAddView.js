var selectRow;

initFunction = function () {
    selectRow = $("#grid").datagrid("getSelected");
}

bindsOrderAddInfo = function () {
    $("#spStatus").html(selectRow.StatusName);
    $("#spName").html(selectRow.Name);
    $("#spDept").html(selectRow.DeptName);
    $("#spYear").html(selectRow.Year);
    $("#spMonth").html(selectRow.Month);
    $("#spMajor").html(selectRow.Major);
    $("#spsFeeScheme").html(selectRow.PlanName);
    $("#spsFeeSemester").html(selectRow.NumName);
    $("#spCreateName").html(selectRow.CreateName);
    $("#spCreateTime").html(selectRow.CreateTime);
    $("#spRemark").html(selectRow.Remark);
}

bindsOrderInfo = function () {
    $("#view_grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        pagination: false,//分页
        showFooter: false,
        pageSize: 20,
        queryParams: { ID: selectRow.sOrderAddID },//异步查询的参数
        columns: [[
            { field: "DetailName", title: "收费项目", width: 60, sortable: true },
            { field: "Sort", title: "项目分类", width: 100, sortable: true },
            { field: "IsGive", title: "类别", width: 60, sortable: true },
            { field: "ShouldMoney", title: "应收金额", width: 60, sortable: true },
            { field: "LimitTime", title: "缴费截止时间", width: 80, sortable: true }
        ]],
        url: "../sOrderAdd/GetsOrderView", sortName: "sOrderAddDetailID", sortOrder: "ASC"
    });
}

tabsLoad = function (i) {
    $('#ViewInfo').tabs({
        border: false,
        selected: i,
        onSelect: function (title, index) {
            if (index == 0) {
                bindsOrderAddInfo();
            }
            else if (index === 1) {
                bindsOrderInfo();
            }
        }
    });
}

initFunction();

tabsLoad(0);