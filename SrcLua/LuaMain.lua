LuaMain = {}
require('Debugger.LuaPanda').start("127.0.0.1",8818)
require('CSDefine')

local sys1 = require("System.Sys1")

function LuaMain.Start()
    sys1.Fun1()
end

return LuaMain
