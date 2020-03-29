initChooseFeeGridTable = function (queryData) {
    $("#chooseFeePlanGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "ItemID", hidden: true },
            { field: "StudentName", title: "学生姓名", width: 70, sortable: true },
            { field: "PlanName", title: "方案名称", width: 230, sortable: true },
            { field: "ProName", title: "专业名称", width: 100, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "EnrollNum", title: "学号", width: 80, sortable: true },
            {
                field: "BeforeEnrollTime", title: "预报名时间", width: 90, sortable: true, formatter: function (value, row, index) {
                    if (value === "1900-01-01")
                        return "";
                    return value;
                }
            },
            {
                field: "EnrollTime", title: "正式报名时间", width: 90, sortable: true, formatter: function (value, row, index) {
                    if (value === "1900-01-01")
                        return "";
                    return value;
                }
            },
            {
                field: "FirstFeeTime", title: "第一次缴费时间", width: 90, sortable: true, formatter: function (value, row, index) {
                    if (value === "1900-01-01")
                        return "";
                    return value;
                }
            },
            { field: "Year", title: "年份", width: 40, sortable: true },
            { field: "Month", title: "月份", width: 40, sortable: true }
        ]],

        sortName: "sEnrollsProfessionID", sortOrder: "desc", url: "../sFee/GetChooseFeePlanList",
        rowStyler: function (index, row) {
        }
    });
}

initChooseFeeGridTable({ DeptID: deptId });

bindSearchChoosePlanClickEvent = function () {
    $("#btnSearch_ChooseFeePlan").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnChoosePlanQueryData();
        initChooseFeeGridTable(queryData);//将值传递给
    });
    $("#btnReset_ChooseFeePlan").click(function () {
        $("#fSearch_ChooseFeePlan").form("reset");//重置表单
        initChooseFeeGridTable({ DeptID: deptId });
    });
}
rerurnChoosePlanQueryData = function () {
    var queueString = {
        DeptID: deptId,
        txtsName: $("#txtsName").textbox("getValue"),
        txtsIDCard: $("#txtsIDCard").textbox("getValue"),
        txtsEnrollNum: $("#txtsEnrollNum").textbox("getValue")
    };
    return queueString;
};

bindSearchChoosePlanClickEvent();