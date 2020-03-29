var isSelected = 0;
firstFunction = function () {
    Easy.bindOpenCloseSearchBoxEvent(90);
    bindSerchForm();
    bindsFeeDetailTable();
    bindSearchClickEvent();
}
bindsFeeDetailTable = function () {
    $("#gridsFeeDetail").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        columns: [[
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "NoteNum", title: "票据号", width: 60, sortable: true },
            { field: "StudentName", title: "学生姓名", width: 80, sortable: true },
            { field: "EnrollNum", title: "学号", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "DeptName", title: "收费学校", width: 200, sortable: true },
            { field: "FeeMode", title: "交费方式", width: 60, sortable: true },
            { field: "LimitTime", title: "应供款日期", width: 80, sortable: true },
            { field: "FeeTime", title: "实际供款日期", width: 80, sortable: true },
            { field: "DetailName", title: "费用类别", width: 120, sortable: true },
            { field: "ShouldMoney", title: "应供款金额", sortable: true, align: 'right', halign: 'left' },
            { field: "PaidMoney", title: "实际供款金额", sortable: true, align: 'right', halign: 'left' },
            { field: "DiscountMoney", title: "优惠金额", sortable: true, align: 'right', halign: 'left' },
            { field: "OffsetMoney", title: "充抵金额", sortable: true, align: 'right', halign: 'left' },
            { field: "ByOffsetMoney", title: "被充抵金额", sortable: true, align: 'right', halign: 'left' },
            { field: "RefundMoney", title: "核销金额", sortable: true, align: 'right', halign: 'left' },
            { field: "Year", title: "年份", width: 60, sortable: true },
            { field: "Month", title: "月份", width: 80, sortable: true },
            { field: "EnrollLevel", title: "学历层次", width: 100, sortable: true },
            { field: "PlanSort", title: "报读类别", width: 80, sortable: true },
            { field: "UserName", title: "收费员", width: 80, sortable: true }
        ]],
        sortName: "sFeesOrderID", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        onLoadSuccess: function (data) {
            $('#gridsFeeDetail').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
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
    $("#selFeeMode").combobox({
        url: "../Refe/SelList?RefeTypeID=6",
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
    $("#treeDetail").combotree({
        url: "../Detail/GetDetailCommonbox",
        animate: true,
        lines: true,
        panelWidth: 300,
        editable: true,
        keyHandler: {
            query: queryHandler
        },
        onHidePanel: function (node) {
            var nodeTree = $('#treeDetail').combotree('tree').tree("getSelected");
            if (nodeTree != null) {
                $("#treeDetail").combotree("setValue", nodeTree.id);
            }
        }
    });
}

queryHandler = function (searchText, event) {
    $('#treeDetail').combotree('tree').tree("search", searchText);
}

derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = getSeachData();
        $.ajax({
            type: "post",
            url: "../sFeeSearch/DownloadsFeesOrderList",
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
    $("#gridsFeeDetail").datagrid({ url: "../sFeeSearch/GetsFeesOrderList", queryParams: data, pageNumber: 1 });
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
        treeDeptID: $("#treeDeptID").combotree("getValue"),
        txtVoucherNum: $("#txtVoucherNum").textbox("getValue"),
        txtNoteNum: $("#txtNoteNum").textbox("getValue"),
        txtEnrollNum: $("#txtEnrollNum").textbox("getValue"),
        txtStudentName: $("#txtStudentName").textbox("getValue"),
        txtIDCard: $("#txtIDCard").textbox("getValue"),
        selYear: "" + $("#selYear").combobox("getValues"),
        selMonth: "" + $("#selMonth").combobox("getValues"),
        selFeeMode: "" + $("#selFeeMode").combobox("getValues"),
        txtShouldFeeTimeS: $("#txtShouldFeeTimeS").datebox("getValue"),
        txtShouldFeeTimeE: $("#txtShouldFeeTimeE").datebox("getValue"),
        txtFeeTimeS: $("#txtFeeTimeS").datebox("getValue"),
        txtFeeTimeE: $("#txtFeeTimeE").datebox("getValue"),
        selEnrollLevel: "" + $("#selEnrollLevel").combobox("getValues"),
        selSort: "" + $("#selSort").combobox("getValues"),
        treeDetail: $("#treeDetail").combotree("getValue"),
        txtFeeName: $("#txtFeeName").textbox("getValue")
    }
    return queueData;
}