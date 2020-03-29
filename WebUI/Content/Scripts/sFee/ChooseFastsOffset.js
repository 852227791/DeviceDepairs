var feeDetailData = JSON.stringify(allDataGrid);
//收费项目列表
bindFeeDetailGrid = function () {
    $("#sfeeDetail").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        columns: [[
            { field: "sOrderID", hidden: true },
            { field: "DetailName", title: "收费类别", width: 115, sortable: true },
            { field: "ShouldMoney", title: "应缴金额", width: 70, sortable: true, align: 'right', halign: 'left' },
        { title: "未缴金额", field: "UnPaid", width: 70, hidden: false }
        ]],
        data: JSON.parse(feeDetailData),
        onClickRow: function (rowIndex, rowData) {
            $("#sFeeOrder").edatagrid({ url: "../sFee/GetChoosesFeeList", queryParams: getQueueData() });
        },
        onLoadSuccess: function (data) {
            setTimeout(function () {
                for (var j = 0; j < totalData.length; j++) {
                    for (var i = 0; i < data.rows.length; i++) {
                        if (data.rows[i]["sOrderID"] === totalData[j]["sOrderID"]) {
                            var row = data.rows[i];
                            var index = $("#sfeeDetail").datagrid('getRowIndex', row);
                            var money = parseFloat(data.rows[i]["UnPaid"]) - (totalData[j]["Offset"]);
                            updateFeeDetailRows(index, money);
                        }
                    }

                }
            }, 1);
        }
    });
};

bindsFeesOffsetGrid = function () {
    $("#sFeesOffset").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行 
        pageSize: 20,
        data: totalData,
        columns: [[
            { field: "sOrderID", hidden: true },
            { field: "ID", hidden: true }, {
                field: "Sort", title: "充抵来源", width: 100, sortable: true,
                formatter: function (value, row, index) {
                    if (value === "1")
                        return "学费";
                    else if (value === "2")
                        return "杂费";
                    else if (value === "3")
                        return "证书"
                }
            },
            { field: "VoucherNum", title: "凭证号", width: 115, sortable: true },
            { field: "Name", title: "学生姓名", width: 80, sortable: true },
            { field: "FeeContent", title: "收费类别", width: 80, sortable: true },
            { field: "Dept", title: "收费单位", width: 80, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 100, sortable: true },
            { field: "Money", title: "充抵金额", width: 100, sortable: true, align: 'right' },
            { field: "OffsetItem", title: "充抵到收费项", width: 100, sortable: true }
        ]]
    });
};
bindsFeeOrderGrid = function () {
    $("#sFeeOrder").edatagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: {},//异步查询的参数
        columns: [[
            { field: "sFeesOrderID", checkbox: true },

            { field: "NumName", title: "缴费次数", width: 80, sortable: true },
            { field: "Offset", title: "充抵金额", width: 80, sortable: false, editor: { type: 'numberspinner', options: { required: true, precision: 2, min: 0 } } },
            { field: "StudName", title: "学生姓名", width: 80, sortable: true },
            { field: "CanMoney", title: "可冲抵金额", sortable: true, align: 'right', halign: 'left' },
            { field: "FeeContent", title: "收费类别", width: 120, sortable: true },
             { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "EnrollNum", title: "学号", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "ShouldMoney", title: "应缴金额", sortable: true, align: 'right', halign: 'left' },
            { field: "PaidMoney", title: "实缴金额", sortable: true, align: 'right', halign: 'left' },
            { field: "DiscountMoney", title: "优惠金额", sortable: true, align: 'right', halign: 'left' },
            { field: "OffsetMoney", title: "充抵金额", sortable: true, align: 'right', halign: 'left' },
            { field: "ByOffsetMoney", title: "被充抵金额", sortable: true, align: 'right', halign: 'left' },
            { field: "RefundMoney", title: "核销金额", sortable: true, align: 'right', halign: 'left' },
            { field: "FeeModel", title: "缴费方式", width: 60, sortable: true },
            { field: "FeeUser", title: "收费人", width: 80, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 90, sortable: true },
            { field: "Affirm", title: "结账人", width: 80, sortable: true, align: 'right', halign: 'left' },
            { field: "AffirmTime", title: "结账时间", width: 90, sortable: true },
            { field: "DeptName", title: "收费单位", width: 200, sortable: true },
            { field: "ProName", title: "专业", width: 120, sortable: true },
            { field: "Status", title: "状态", width: 60, sortable: true },
            { field: "NoteNum", title: "票据号", width: 60, sortable: true }
        ]],
        sortName: "FeeTime", sortOrder: "desc",
        onLoadSuccess: function (data) {
            var offsetData = $("#sFeesOffset").datagrid("getData");
            for (var j = 0; j < offsetData.rows.length; j++) {

                for (var i = 0; i < data.rows.length; i++) {
                    if (data.rows[i]["sFeesOrderID"] === offsetData.rows[j]["sFeesOrderID"]) {
                        var row = data.rows[i];
                        var index = $("#sFeeOrder").datagrid('getRowIndex', row);
                        var money = parseFloat(data.rows[i]["CanMoney"]) - (offsetData.rows[j]["Offset"]);
                        updateRows(index, money, "0.00");
                    }
                }

            }
        }
    });
}


//获取查询参数
var getQueueData = function () {
    var queueData = {
        studName: $("#txtChoStudName").textbox("getValue"),
        voucherNum: $("#txtChoVoucherNum").textbox("getValue"),
        idCard: $("#txtChoIdCard").textbox("getValue"),
       
    }
    return queueData;
}
bindAddMethod = function () {
    $("#btnAddsOffset").click(function () {
        $("#sFeeOrder").datagrid('acceptChanges');
        var row = $("#sFeeOrder").datagrid("getSelected");//获取选中行
        var index = $("#sFeeOrder").datagrid('getRowIndex', row);//获取选中行索引

        var itemdetial = $("#sfeeDetail").datagrid("getSelected");
        var orderIndex = $("#sfeeDetail").datagrid('getRowIndex', itemdetial);

        if (row != null && row != undefined) {
            if (itemdetial.length <= 0) {
                Easy.centerShow("系统消息", "请选择收费项目名称！", 1000);
                return false;
            }
            if (parseFloat(row.Offset) > parseFloat(row.CanMoney)) {
                Easy.centerShow("系统消息", "充抵金额不能大于可充抵金额！", 1000);
                return false;
            }
            if (row.Offset === undefined) {
                Easy.centerShow("系统消息", "充抵金额不能为空！", 1000);
                return false;
            }
            if (parseFloat(row.Offset) === 0) {
                Easy.centerShow("系统消息", "充抵金额不能为0！", 1000);
                return false;
            }
            var money = itemdetial.UnPaid;
            if (parseFloat(row.Offset) > parseFloat(money)) {
                Easy.centerShow("系统消息", "充抵金额不能超过该收费项的金额！", 1000);
                return false;
            }
            var data = $('#sFeesOffset').datagrid("getData");
            for (var i = 0; i < data.rows.length; i++) {
                if (row.sFeesOrderID === data.rows[i]["sFeesOrderID"] && itemdetial.sOrderID === data.rows[i]["sOrderID"]) {
                    Easy.centerShow("系统消息", "同一收费项不能充抵两次给同一个收费项！", 1000);
                    return false;
                }
            }
            $('#sFeesOffset').datagrid('appendRow',
             {
                 sFeesOrderID: row.sFeesOrderID,
                 VoucherNum: row.VoucherNum,
                 Name: row.StudName,
                 Dept: row.DeptName,
                 NoteNum: row.NoteNum,
                 IDCard: row.IDCard,
                 FeeTime: row.FeeTime,
                 CanMoney: row.CanMoney,
                 Offset: row.Offset,
                 sOrderID: itemdetial.sOrderID,
                 OffsetItem: itemdetial.DetailName,
                 FeeContent: row.FeeContent

             });
            var money = row.CanMoney - row.Offset;
            var orderMoney = itemdetial.UnPaid - row.Offset;
            updateRows(index, money, "0.00");
            updateOrderRows(orderIndex, orderMoney);

        }
        else {
            Easy.centerShow("系统消息", "请选择收费信息！", 1000);
        }

    });
}

updateRows = function (index, money, offset) {
    $("#sFeeOrder").datagrid('updateRow', {
        index: index,
        row: {
            CanMoney: parseFloat(money).toFixed("2"),
            Offset: parseFloat(offset).toFixed("2")
        }
    });
}
updateFeeDetailRows = function (index, money) {
    $("#sfeeDetail").datagrid('updateRow', {
        index: index,
        row: {
            UnPaid: parseFloat(money).toFixed("2")
        }
    });
}
updateOrderRows = function (index, money) {
    $("#sfeeDetail").datagrid('updateRow', {
        index: index,
        row: {
            UnPaid: parseFloat(money).toFixed("2")
        }
    });
}

bindRemoveMethod = function () {
    $("#btnRemovesOffset").click(function () {
        var row = $('#sFeesOffset').datagrid("getSelected");
        if (row != null) {
            var ix0 = $('#sFeesOffset').datagrid('getRowIndex', row);
            var data = $("#sFeeOrder").datagrid("getData");
            for (var i = 0; i < data.rows.length; i++) {
                var newrow = data.rows[i];
                var index = $("#sFeeOrder").datagrid('getRowIndex', newrow);
                if (newrow.sFeesOrderID === row.sFeesOrderID) {
                    var money = parseFloat(newrow.CanMoney) + parseFloat(row.Offset);
                    updateRows(index, money, "0.00");
                }
            }
            var orderData = $("#sfeeDetail").datagrid("getData");
            for (var i = 0; i < orderData.rows.length; i++) {
                if (orderData.rows[i].sOrderID === row.sOrderID) {
                    var orderMoney = parseFloat(orderData.rows[i].UnPaid) + parseFloat(row.Offset);
                    updateOrderRows(i, orderMoney);
                }
            }
            $('#sFeesOffset').datagrid('deleteRow', ix0);
        } else {
            Easy.centerShow("系统消息", "请选择收费信息！", 1000);
        }
    });
}
bindButtonMethod = function () {
    $("#btnSubmitForm").click(function () {
        var rows = $("#sfeeDetail").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费类别")) {
            var queryData = getQueueData();
            $("#sFeeOrder").datagrid({
                url: "../sFee/GetChoosesFeeList",
                queryParams: queryData
            });
        }

    });

    $("#btnResetForm").click(function () {
        var rows = $("#sfeeDetail").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "收费类别")) {
            $("#sOffsetForm").form("reset");//重置表单
            $("#sFeeOrder").datagrid({
                url: "../sFee/GetChoosesFeeList",
                queryParams: {}
            });
        }
    });
};
bindFeeDetailGrid();
bindsFeeOrderGrid();
bindsFeesOffsetGrid();
bindAddMethod();
bindRemoveMethod();
bindButtonMethod();