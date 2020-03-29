var isSelected = 0;
firstFunction = function () {
    Easy.bindOpenCloseSearchBoxEvent(120);
    bindSerchForm();
    bindTabEvent();
    bindsFeeALLTable();
    bindsFeeDetailTable();
    bindSearchClickEvent();
    bindsFeeYearTable();
}
bindsFeeALLTable = function () {
    $("#gridAll").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        columns: [[
            { field: "Status", title: "在校状态", width: 60, sortable: true },
            { field: "EnrollNum", title: "学号", width: 100, sortable: true },
            { field: "StudentName", title: "学生姓名", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "DeptName", title: "就读学校", width: 200, sortable: true },
            { field: "PlanSort", title: "报读类别", width: 80, sortable: true },
            { field: "ProfessionName", title: "报读专业", width: 120, sortable: true },
            { field: "ClassName", title: "班级", width: 80, sortable: true },
            { field: "BeforeEnrollTime", title: "预报名日期", width: 80, sortable: true },
            { field: "EnrollTime", title: "正式报名日期", width: 80, sortable: true },
            { field: "FirstFeeTime", title: "首次缴费日期", width: 80, sortable: true },
            { field: "ShouldMoney", title: "总学费金额", sortable: true, align: 'right', halign: 'left' },
            { field: "PaidMoney", title: "已供贷金额", sortable: true, align: 'right', halign: 'left' },
            {
                field: "NoMoney", title: "未供贷金额", sortable: true, align: 'right', halign: 'left', formatter: function (value, row, index) {
                    return (parseFloat(row.ShouldMoney) - parseFloat(row.PaidMoney)).toFixed(2);
                }
            },
            { field: "ArrearsMoney", title: "逾期欠费金额", sortable: true, align: 'right', halign: 'left' },
            { field: "ArrearsMoneyGive", title: "逾期欠费(补贴)", sortable: true, align: 'right', halign: 'left' },
            { field: "Year", title: "年份", width: 60, sortable: true },
            { field: "Month", title: "月份", width: 80, sortable: true },
            { field: "EnrollLevel", title: "学历层次", width: 100, sortable: true },
            {
                field: "ArrearsStatus", title: "逾期状态", width: 60, sortable: false, formatter: function (value, row, index) {
                    if (parseFloat(row.ShouldMoney) === 0) {
                        return "未收";
                    } else if (parseFloat(row.ShouldMoney) - parseFloat(row.PaidMoney) === 0) {
                        return "<span style=color:#339900>已清</span>";
                    } else if (parseFloat(row.ArrearsMoney) === 0 && parseFloat(row.ArrearsMoneyGive) === 0) {
                        return "<span style=color:#1d5ea0>正常</span>";
                    } else {
                        return "<span style=color:#ff0000>逾期</span>";
                    }
                }
            }
        ]],
        sortName: "sEnrollsProfessionID", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        onLoadSuccess: function (data) {
            $('#gridAll').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
        }
    });
};
bindsFeeDetailTable = function () {
    $("#gridDetail").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        columns: [[
            { field: "Status", title: "在校状态", width: 60, sortable: true },
            { field: "EnrollNum", title: "学号", width: 100, sortable: true },
            { field: "StudentName", title: "学生姓名", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "DeptName", title: "就读学校", width: 200, sortable: true },
            { field: "PlanSort", title: "报读类别", width: 80, sortable: true },
            { field: "ProfessionName", title: "报读专业", width: 120, sortable: true },
            { field: "ClassName", title: "班级", width: 80, sortable: true },
            { field: "BeforeEnrollTime", title: "预报名日期", width: 80, sortable: true },
            { field: "EnrollTime", title: "正式报名日期", width: 80, sortable: true },
            { field: "FirstFeeTime", title: "首次缴费日期", width: 80, sortable: true },
            { field: "LimitTime", title: "应供贷日期", width: 80, sortable: true },
            { field: "NumName", title: "缴费学年", width: 80, sortable: true },
            { field: "DetailName", title: "费用类别", width: 120, sortable: true },
            { field: "ShouldMoney", title: "应供贷金额", sortable: true, align: 'right', halign: 'left' },
            { field: "PaidMoney", title: "已供贷金额", sortable: true, align: 'right', halign: 'left' },
            { field: "NoMoney", title: "未供贷金额", sortable: true, align: 'right', halign: 'left' },
            { field: "Year", title: "年份", width: 60, sortable: true },
            { field: "Month", title: "月份", width: 80, sortable: true },
            { field: "EnrollLevel", title: "学历层次", width: 100, sortable: true },
            { field: "ArrearsStatus", title: "逾期状态", width: 60, sortable: true }
        ]],
        sortName: "sOrderID", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "2"),
        onLoadSuccess: function (data) {
            $('#gridDetail').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
        }
    });
};
bindsFeeYearTable = function () {
    $("#gridYear").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        pagination: true,//分页
        showFooter: true,
        pageSize: 20,
        columns: [[
            { field: "Status", title: "在校状态", width: 60, sortable: true },
            { field: "EnrollNum", title: "学号", width: 100, sortable: true },
            { field: "StudentName", title: "学生姓名", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "DeptName", title: "就读学校", width: 200, sortable: true },
            { field: "PlanSort", title: "报读类别", width: 80, sortable: true },
            { field: "ProfessionName", title: "报读专业", width: 120, sortable: true },
            { field: "ClassName", title: "班级", width: 80, sortable: true },
            { field: "BeforeEnrollTime", title: "预报名日期", width: 80, sortable: true },
            { field: "EnrollTime", title: "正式报名日期", width: 80, sortable: true },
            { field: "FirstFeeTime", title: "首次缴费日期", width: 80, sortable: true },
            { field: "LimitTime", title: "应供贷日期", width: 80, sortable: true },
            { field: "NumName", title: "缴费学年", width: 80, sortable: true },
            { field: "ShouldMoney", title: "应供贷金额", sortable: true, align: 'right', halign: 'left' },
            { field: "PaidMoney", title: "已供贷金额", sortable: true, align: 'right', halign: 'left' },
            {
                field: "NoMoney", title: "未供贷金额", sortable: true, align: 'right', halign: 'left', formatter: function (value, row, index) {
                    return (parseFloat(row.ShouldMoney) - parseFloat(row.PaidMoney)).toFixed(2);
                }
            },
            { field: "ArrearsMoney", title: "逾期欠费金额", sortable: true, align: 'right', halign: 'left' },
            { field: "ArrearsMoneyGive", title: "逾期欠费(补贴)", sortable: true, align: 'right', halign: 'left' },
            { field: "Year", title: "年份", width: 60, sortable: true },
            { field: "Month", title: "月份", width: 80, sortable: true },
            { field: "EnrollLevel", title: "学历层次", width: 100, sortable: true },
            {
                field: "ArrearsStatus", title: "逾期状态", width: 60, sortable: false, formatter: function (value, row, index) {
                    if (parseFloat(row.ShouldMoney) === 0) {
                        return "未收";
                    } else if (parseFloat(row.ShouldMoney) - parseFloat(row.PaidMoney) === 0) {
                        return "<span style=color:#339900>已清</span>";
                    } else if (parseFloat(row.ArrearsMoney) === 0 && parseFloat(row.ArrearsMoneyGive) === 0) {
                        return "<span style=color:#1d5ea0>正常</span>";
                    } else {
                        return "<span style=color:#ff0000>逾期</span>";
                    }
                }
            }
        ]],
        sortName: "sOrderID", sortOrder: "asc",
        toolbar: Easy.loadToolbar(menuId, "3"),
        onLoadSuccess: function (data) {
            $('#gridYear').resizeDataGrid(0, 0, 0, 0);//重新计算宽度高度
        }
    });
};

bindTabEvent = function () {
    $('#gridTab').tabs({
        border: false,
        selected: isSelected,
        onSelect: function (title, index) {
            if (index === 0) {
                $("#gridAll").datagrid({ url: "../sFeeSearch/GetsFeeAllList", queryParams: getSeachData(), pageNumber: 1 });
            } else if (index === 1) {
                $("#gridYear").datagrid({ url: "../sFeeSearch/GetOrderByYear", queryParams: getSeachData(), pageNumber: 1 });
            }
            else if (index === 2) {
                $("#gridDetail").datagrid({ url: "../sFeeSearch/GetsFeeDetailList", queryParams: getSeachData(), pageNumber: 1 });
            }

        }
    });
};



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
    $("#selArrearsStatus").combobox({
        data: [{
            Id: '1',
            Text: '已清'
        }, {
            Id: '2',
            Text: '逾期'
        }, {
            Id: '3',
            Text: '正常'
        }, {
            Id: '4',
            Text: '未收'
        }],
        valueField: "Id",
        textField: "Text",
        multiple: true,
        editable: false
    });
}

queryHandler = function (searchText, event) {
    $('#treeDetail').combotree('tree').tree("search", searchText);
}
deriveAll = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveAll")) {
        var queryData = getSeachData();
        $.ajax({
            type: "post",
            url: "../sFeeSearch/DownloadsFeeAllList",
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
};
deriveDetail = function () {
    if (Easy.bindPowerValidationEvent(menuId, "2", "deriveDetail")) {
        var queryData = getSeachData();
        $.ajax({
            type: "post",
            url: "../sFeeSearch/DownloadsFeeDetailList",
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
};
downbyYear = function () {
    if (Easy.bindPowerValidationEvent(menuId, "3", "downbyYear")) {
        var queryData = getSeachData();
        $.ajax({
            type: "post",
            url: "../sFeeSearch/DownloadsFeeByYear",
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
    var tab = $('#gridTab').tabs('getSelected');
    var index = $('#gridTab').tabs('getTabIndex', tab);
    if (index === 0) {
        $("#gridAll").datagrid({ url: "../sFeeSearch/GetsFeeAllList", queryParams: data, pageNumber: 1 });
    }
    else if (index === 1) {
        $("#gridYear").datagrid({ url: "../sFeeSearch/GetOrderByYear", queryParams: getSeachData(), pageNumber: 1 });

    }
    else if (index === 2) {
        $("#gridDetail").datagrid({ url: "../sFeeSearch/GetsFeeDetailList", queryParams: data, pageNumber: 1 });
    }
};
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
        txtEnrollNum: $("#txtEnrollNum").textbox("getValue"),
        txtStudentName: $("#txtStudentName").textbox("getValue"),
        txtIDCard: $("#txtIDCard").textbox("getValue"),
        selYear: "" + $("#selYear").combobox("getValues"),
        selMonth: "" + $("#selMonth").combobox("getValues"),
        txtBeforeEnrollTimeS: $("#txtBeforeEnrollTimeS").datebox("getValue"),
        txtBeforeEnrollTimeE: $("#txtBeforeEnrollTimeE").datebox("getValue"),
        txtEnrollTimeS: $("#txtEnrollTimeS").datebox("getValue"),
        txtEnrollTimeE: $("#txtEnrollTimeE").datebox("getValue"),
        txtFirstFeeTimeS: $("#txtFirstFeeTimeS").datebox("getValue"),
        txtFirstFeeTimeE: $("#txtFirstFeeTimeE").datebox("getValue"),
        selStatus: "" + $("#selStatus").combobox("getValues"),
        txtProfessionName: $("#txtProfessionName").textbox("getValue"),
        selEnrollLevel: "" + $("#selEnrollLevel").combobox("getValues"),
        selSort: "" + $("#selSort").combobox("getValues"),
        treeDetail: $("#treeDetail").combotree("getValue"),
        txtArrearsMoneyS: $("#txtArrearsMoneyS").numberspinner("getValue"),
        txtArrearsMoneyE: $("#txtArrearsMoneyE").numberspinner("getValue"),
        txtPaidMoneyS: $("#txtPaidMoneyS").numberspinner("getValue"),
        txtPaidMoneyE: $("#txtPaidMoneyE").numberspinner("getValue"),
        txtLimitTimeS: $("#txtLimitTimeS").datebox("getValue"),
        txtLimitTimeE: $("#txtLimitTimeE").datebox("getValue"),
        selArrearsStatus: "" + $("#selArrearsStatus").combobox("getValues")
    }
    return queueData;
};
