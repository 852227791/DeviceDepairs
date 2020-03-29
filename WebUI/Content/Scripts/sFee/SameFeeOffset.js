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
        onLoadSuccess: function (data) {
            setTimeout(function () {
                for (var j = 0; j < totalData.length; j++) {
                    for (var i = 0; i < data.rows.length; i++) {
                        if (data.rows[i]["sOrderID"] === totalData[j]["sOrderID"]) {
                            var row = data.rows[i];
                            var index = $("#sfeeDetail").datagrid('getRowIndex', row);
                            var unpaid = parseFloat(data.rows[i]["UnPaid"]);
                            var offset = parseFloat(totalData[j]["Money"]);
                            var money = (unpaid - offset).toFixed("2");
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
            { field: "ID", hidden: true },
            { field: "Sort", hidden: true },
            { field: "Name", title: "学生姓名", width: 70, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "OffsetItem", title: "充抵到收费项", width: 100, sortable: true },
            { field: "Money", title: "充抵金额", width: 60, sortable: true, align: 'right', halign: 'left' },
            { field: "VoucherNum", title: "被充抵凭证号", width: 115, sortable: true },
            { field: "FeeContent", title: "被充抵收费类", width: 100, sortable: true },
            {
                field: "SortName", title: "被充抵类别", width: 70, sortable: true, formatter: function (value, row, index) {
                    if (row.Sort === "1") {
                        return "学费";
                    } else if (row.Sort === "2") {
                        return "杂费";
                    } else if (row.Sort === "3") {
                        return "证书费";
                    }
                }
            }
        ]],
        toolbar: [{
            text: '充抵',
            iconCls: 'icon-edit',
            handler: function () {
                bindOffsetMethod();
            }
        }]
    });
};
bindsFeeOrderGrid = function () {
    $("#sFeeOrder").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        data: JSON.parse(sfeeOrderdata),//异步查询的参数
        columns: [[
            { field: "ID", checkbox: true },
            { field: "CanMoney", title: "可充抵金额", sortable: true, align: 'right', width: 100 },
            { field: "StudName", title: "学生姓名", width: 60, sortable: true },
            { field: "FeeContent", title: "收费类别", width: 80, sortable: true },
            { field: "NumName", title: "缴费学年", width: 60, sortable: true },
            { field: "ProName", title: "专业", width: 120, sortable: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "EnrollNum", title: "学号", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "DeptName", title: "收费单位", width: 200, sortable: true },
            { field: "NoteNum", title: "票据号", width: 60, sortable: true },
            { field: "Sort", hidden: true }
        ]],
        sortName: "FeeTime", sortOrder: "desc",
        onLoadSuccess: function (data) {
            setTimeout(function () {
                for (var j = 0; j < totalData.length; j++) {
                    for (var i = 0; i < data.rows.length; i++) {
                        if (data.rows[i]["ID"] === totalData[j]["ID"]) {
                            var row = data.rows[i];
                            var index = $("#sFeeOrder").datagrid('getRowIndex', row);
                            var unpaid = parseFloat(data.rows[i]["CanMoney"]);
                            var offset = parseFloat(totalData[j]["Money"]);
                            var money = (unpaid - offset).toFixed("2");
                            $("#sFeeOrder").datagrid('updateRow', {
                                index: index,
                                row: {
                                    CanMoney: parseFloat(money).toFixed("2")
                                }
                            });
                        }
                    }

                }
            }, 1);
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
bindRemove = function () {
    $("#sFeesOffset").datagrid({ data: [] });
};
bindOffsetMethod = function () {
    var offsetData = $("#sFeeOrder").datagrid("getData");
    var orderData = $("#sfeeDetail").datagrid("getData");

    for (var i = 0; i < offsetData.rows.length; i++) {
        var row = offsetData.rows[i];
        var index = $("#sFeeOrder").datagrid('getRowIndex', row);
        for (var j = 0; j < orderData.rows.length; j++) {

            var itemdetial = orderData.rows[j]
            if (itemdetial.DetailName === row.FeeContent) {
                var orderIndex = $("#sfeeDetail").datagrid('getRowIndex', itemdetial);
                if (parseFloat(row.CanMoney) > 0) {
                    $('#sFeesOffset').datagrid('appendRow',
                      {
                          ID: row.ID,
                          VoucherNum: row.VoucherNum,
                          Name: row.StudName,
                          IDCard: row.IDCard,
                          Money: parseFloat(row.CanMoney).toFixed("2"),
                          sOrderID: itemdetial.sOrderID,
                          OffsetItem: itemdetial.DetailName,
                          FeeContent: row.FeeContent,
                          Sort: row.Sort
                      });
                    var orderMoney = itemdetial.UnPaid - row.CanMoney;
                    updateOrderRows(orderIndex, orderMoney);
                }

            }
        }
        updateRows(index, "0.00", "0.00");
    }

}

bindFeeDetailGrid();
bindsFeeOrderGrid();
bindsFeesOffsetGrid();
bindRemoveMethod();
bindButtonMethod();