bindSelectInfo = function () {
    if (ProfessionID != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Profession/SelectProfession", ProfessionID);
            var p = JSON.parse(result.Message)[0];
            $("#ProfessionID").textbox("setValue", p.ProfessionID);
            $("#DeptID").textbox("setValue", p.DeptID);
            $("#ProfessionName").textbox("setValue", p.Name);
            $("#EnglishName").textbox("setValue", p.EnglishName);
        }, 1);
    }
    else {
        $("#DeptID").val(DeptID);
    }
}
bindSelectInfo();