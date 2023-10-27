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
	local protoc = require "ProtoBuf.protoc"

	-- 直接载入schema (这么写只是方便, 生产环境推荐使用 protoc.new() 接口)
	assert(protoc:load [[
	   message Phone {
		  optional string name        = 1;
		  optional int64  phonenumber = 2;
	   }
	   message Person {
		  optional string name     = 1;
		  optional int32  age      = 2;
		  optional string address  = 3;
		  repeated Phone  contacts = 4;
	   } ]])

	-- lua 表数据
	local data = {
	   name = "ilse",
	   age  = 18,
	   contacts = {
		  { name = "alice", phonenumber = 12312341234 },
		  { name = "bob",   phonenumber = 45645674567 }
	   }
	}

	-- 将Lua表编码为二进制数据
	local bytes = assert(pb.encode("Person", data))
	print(pb.tohex(bytes))

	-- 再解码回Lua表
	local data2 = assert(pb.decode("Person", bytes))
	print(require "ProtoBuf.serpent".block(data2))

	
end

return LuaMain
