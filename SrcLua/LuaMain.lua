LuaMain = {}
require('Debugger.LuaPanda').start("127.0.0.1",8818)
require('CSDefine')

local sys1 = require("System.Sys1")

function LuaMain.Start()
    sys1.Fun1()
	
	local rapidjson = require('rapidjson')
	local t = rapidjson.decode('{"a":123}')
	print(t.a)
	t.a = 456
	local s = rapidjson.encode(t)
	print('json', s)
	
	
	local pb = require "pb"
	assert(pb.loadfile "login.pb")
	local loginCS = {
		username="jack",
		password="123456"
	}
	local bytes = assert(pb.encode("login.req_login", loginCS))
	print(pb.tohex(bytes))
	local recvData = assert(pb.decode("login.req_login", bytes))
	print(recvData .username)
	
end

return LuaMain
