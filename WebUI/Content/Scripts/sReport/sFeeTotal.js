var isSelected = 0;
firstFunction = function () {
    Easy.bindOpenCloseSearchBoxEvent(120);
    bindSerchForm();
    bindsFeeDetailTable();
    bindSearchClickEvent();
}
bindsFeeDetailTable = function () {
    $("#gridsFeeTotal").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        columns: [[
            { field: "Status", title: "在校状态", width: 60, sortable: true },
            { field: "StudentName", title: "学生姓名", width: 80, sortable: true },
            { field: "EnrollNum", title: "学号", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "DeptName", title: "报读学校", width: 200, sortable: true },
            { field: "PlanSort", title: "报读类别", width: 80, sortable: true },
            { field: "ProfessionName", title: "报读专业", width: 120, sortable: true },
            { field: "ClassName", title: "班级", width: 80, sortable: true },
            { field: "BeforeEnrollTime", title: "预报名日期", width: 80, sortable: true },
            { field: "EnrollTime", title: "正式报名日期", width: 80, sortable: true },
            { field: "FirstFeeTime", title: "首次缴费日期", width: 80, sortable: true },
            { field: "PaidMoney", title: "供款金额", sortable: true, align: 'right', halign: 'left' },
            { field: "DiscountMoney", title: "优惠金额", sortable: true, align: 'right', halign: 'left' },
            { field: "Year", title: "年份", width: 60, sortable: true },
            { field: "Month", title: "月份", width: 80, sortable: true },
            { field: "EnrollLevel", title: "学历层次", width: 100, sortable: true }
        ]],
        sortName: "sEnrollsProfessionID", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        onLoadSuccess: function (data) {
            $('#gridsFeeTotal').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
        }
    });
}

bindSerchForm = function () {
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
    $("#treeDeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
    });
    $("#selStatus").combobox({
        url: "../Refe/SelList?RefeTypeID=21",
        valueField: "Value",
        textField: "RefeName",
        multiple: true,
        editable: false
    });
    $("#selEnrollLevel").combobox({
        url: "../Refe/SelList?RefeTypeID=17",
        valueField: "Value",
        textField: "RefeName",
        multiple: true,
        editable: false,
        panelWidth: 100
    });
    $("#selSort").combobox({
        url: "../Refe/SelList?RefeTypeID=14",
        valueField: "Value",
        textField: "RefeName",
        multiple: true,
        editable: false,
        panelWidth: 100
    });
}

queryHandler = function (searchText, event) {
    $('#treeDetail').combotree('tree').tree("search", searchText);
}

download = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "download")) {
        var queryData = getSeachData();
        $.ajax({
            type: "post",
            url: "../sFeeSearch/DownloadsFeeTotalList",
            async: false,
            data: queryData,
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    location.href = "../Temp/" + result.Message;
                }
            }
        });
    }
}

bindSelect = function (data) {
    $("#gridsFeeTotal").datagrid({ url: "../sFeeSearch/GetsFeeTotalList", queryParams: data, pageNumber: 1 });
}
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var data = getSeachData();
        bindSelect(data);
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        bindSelect({ MenuID: menuId });
    });
}
var getSeachData = function () {
    var queueData = {
        MenuID: menuId,
        txtEnrollNum: $("#txtEnrollNum").textbox("getValue"),
        txtStudentName: $("#txtStudentName").textbox("getValue"),
        txtIDCard: $("#txtIDCard").textbox("getValue"),
        selYear: "" + $("#selYear").combobox("getValues"),
        selMonth: "" + $("#selMonth").combobox("getValues"),
        txtFeeTimeS: $("#txtFeeTimeS").datebox("getValue"),
        txtFeeTimeE: $("#txtFeeTimeE").datebox("getValue"),
        treeDeptID: $("#treeDeptID").combotree("getValue"),
        selStatus: "" + $("#selStatus").combobox("getValues"),
        txtBeforeEnrollTimeS: $("#txtBeforeEnrollTimeS").datebox("getValue"),
        txtBeforeEnrollTimeE: $("#txtBeforeEnrollTimeE").datebox("getValue"),
        txtEnrollTimeS: $("#txtEnrollTimeS").datebox("getValue"),
        txtEnrollTimeE: $("#txtEnrollTimeE").datebox("getValue"),
        txtFirstFeeTimeS: $("#txtFirstFeeTimeS").datebox("getValue"),
        txtFirstFeeTimeE: $("#txtFirstFeeTimeE").datebox("getValue"),
        txtProfessionName: $("#txtProfessionName").textbox("getValue"),
        selEnrollLevel: "" + $("#selEnrollLevel").combobox("getValues"),
        selSort: "" + $("#selSort").combobox("getValues"),
        txtMoneyS: $("#txtMoneyS").numberspinner("getValue"),
        txtMoneyE: $("#txtMoneyE").numberspinner("getValue")
    }
    return queueData;
}