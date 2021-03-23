# SwipeCarBase
인천정보과학고 전산과 게임프로그래밍 수업을 위해 제작되었습니다. 학생들은 본 프로젝트를 포크(Fork)하여 사용하시기 바랍니다. 
여기에 자신의 프로젝트를 소개해보세요.


-- Counter Blox GUI 스크립트

-- Checks if script is already executed
if getrenv().meiosis then
    return
end
getrenv().meiosis = true

-- Services
local plrs = game:GetService("Players")
local plr = plrs.LocalPlayer
local repstor = game:GetService("ReplicatedStorage")
local runserv = game:GetService("RunService")
local uis = game:GetService("UserInputService")

-- Variables
local cam = workspace.CurrentCamera
local client = getsenv(plr.PlayerGui.Client)
local espFolder = Instance.new("Folder",game.CoreGui)

-- Cheat Variables
local rage = false
local shootCooldown = false
local antiAim = "OFF"
local visual = false
local plrChams = false
local bulletTracer = false
local thirdPerson = false
local hoop = nil
local target = nil

-- Cheat Backups
local speedBackup = client.speedupdate

-- Metatables
local mt = getrawmetatable(game)
setreadonly(mt,false)
local nc = mt.__namecall

mt.__namecall = newcclosure(
	function(self,...)
		local args = {...}
		local namecall = getnamecallmethod()
		if namecall == "FindPartOnRayWithIgnoreList" then
			if target and plr.Character and rage then
				args[1] = Ray.new(workspace.CurrentCamera.CFrame.p, (target.CFrame.p - workspace.CurrentCamera.CFrame.p).unit * 500)
			end
		end
		if namecall == "FireServer" and tostring(self) == "ControlTurn" then
			if plr.Character and rage and antiAim then
				args[1] = -0.96247750520706
			end
		end
		return nc(self,unpack(args))
	end
)

-- Cheat Functions
function simulateShot(target)
	local hitList = {
		cam,
		plr.Character,
		game.Workspace.Debris,
		game.Workspace.Ray_Ignore,
		game.Workspace.Map:WaitForChild("Clips"),
		game.Workspace.Map:WaitForChild("SpawnPoints")
	}
	local gun = client.gun
	local gunPen,gunRange,gunDMG
	if gun:FindFirstChild("Penetration") then
		gunPen = gun.Penetration.Value * 0.01
	end
	if gun:FindFirstChild("Range") then
		gunRange = gun.Range.Value
	end
	if gun:FindFirstChild("DMG") then
		gunDMG = gun.DMG.Value
	end
	local direction = CFrame.new(cam.CFrame.p, target.Character.Head.Position).lookVector.unit * gunRange * 0.0694
	local rayCasted = Ray.new(cam.CFrame.p, direction)
	local hitPart, hitPos = workspace:FindPartOnRayWithIgnoreList(rayCasted, hitList, false, true)
	local partPenetrated = 0
	local limit = 0
	local partHit, posHit, normHit
	local partModifier = 1
	local damageModifier = 1
	repeat
		partHit, posHit, normHit = workspace:FindPartOnRayWithIgnoreList(rayCasted, hitList, false, true)
		if partHit and partHit.Parent then
			partModifier = 1
			if partHit.Material == Enum.Material.DiamondPlate then
				partModifier = 3
			elseif partHit.Material == Enum.Material.CorrodedMetal or partHit.Material == Enum.Material.Metal or partHit.Material == Enum.Material.Concrete or partHit.Material == Enum.Material.Brick then
				partModifier = 2
			elseif partHit.Name == "Grate" or partHit.Material == Enum.Material.Wood or partHit.Material == Enum.Material.WoodPlanks or partHit and partHit.Parent and partHit.Parent:FindFirstChild("Humanoid") then
				partModifier = 0.1
			elseif partHit.Transparency == 1 or partHit.CanCollide == false or partHit.Name == "Glass" or partHit.Name == "Cardboard" or partHit:IsDescendantOf(game.Workspace.Ray_Ignore) or partHit:IsDescendantOf(game.Workspace.Debris) or partHit and partHit.Parent and partHit.Parent.Name == "Hitboxes" then
				partModifier = 0
			elseif partHit.Name == "nowallbang" then
				partModifier = 100
			elseif partHit:FindFirstChild("PartModifier") then
				partModifier = partHit.PartModifier.Value
			end
			local fakeHit, fakePos = workspace:FindPartOnRayWithWhitelist(Ray.new(posHit + direction * 1, direction * -2), {partHit}, true)
			local penDistance = (fakePos - posHit).magnitude
			penDistance = penDistance * partModifier
			limit = math.min(gunPen, limit + penDistance)
			local wallbang = false
			if partPenetrated >= 1 then
				wallbang = true
			end
			if partHit and partHit.Parent and partHit.Parent.Name == "Hitboxes" or partHit and partHit.Parent.className == "Accessory" or partHit and partHit.Parent.className == "Hat" or partHit.Name == "HumanoidRootPart" and partHit.Parent.Name ~= "Door" or partHit.Name == "Head" and partHit.Parent:FindFirstChild("Hostage") == nil then
			else
				if partHit and partHit:IsDescendantOf(target.Character) and partHit.Transparency < 1 or partHit.Name == "HeadHB" then
					return partHit, posHit, damageModifier, wallbang
				end
			end
			damageModifier = 1 - limit / gunPen
			if partModifier > 0 then
				partPenetrated = partPenetrated + 1
			end
			if partHit and partHit.Parent and partHit.Parent.Name == "Hitboxes" or partHit and partHit.Parent and partHit.Parent.Parent and partHit.Parent.Parent:FindFirstChild("Humanoid2") or partHit and partHit.Parent and partHit.Parent:FindFirstChild("Humanoid2") or partHit and partHit.Parent and partHit.Parent:FindFirstChild("Humanoid") and (1 > partHit.Transparency or partHit.Name == "HeadHB") and partHit.Parent:IsA("Model") then
				table.insert(hitList, partHit.Parent)
			else
				table.insert(hitList, partHit)
			end
		end
	until partHit == nil or partHit.Parent == target.Character or limit >= gunPen or 0 >= damageModifier or partPenetrated >= 4
end

function AA(angle) -- Pasted from RobloxForums
	if plr.Character and plr.Character:FindFirstChild("HumanoidRootPart") then
		plr.Character.HumanoidRootPart.CFrame = CFrame.new(plr.Character.HumanoidRootPart.Position, plr.Character.HumanoidRootPart.Position + Vector3.new(cam.CFrame.lookVector.X, 0, cam.CFrame.lookVector.Z)) * CFrame.Angles(0, math.rad(angle), 0)
	end
end

function chamPlr(plr)
	if plr.Character then
		for i,v in next,plr.Character:GetChildren() do
			if v:IsA("BasePart") and v.Name ~= "HumanoidRootPart" then
				local chamThing = Instance.new("BoxHandleAdornment",espFolder)
				chamThing.Size = v.Size
				chamThing.Transparency = 0.5
				chamThing.ZIndex = 0
				chamThing.AlwaysOnTop = true
				chamThing.Visible = true
				chamThing.Adornee = v
				chamThing.Color3 = Color3.fromRGB(255, 85, 0)
				plr.Character.HumanoidRootPart.AncestryChanged:connect(function()
					chamThing:Destroy()
				end)
			end
		end
	end
end

function traceBullet(pos)
	local tracer = Instance.new("Part",game.Workspace.Debris)
	tracer.BrickColor = Color3.fromRGB(255, 85, 0)
	tracer.Material = Enum.Material.ForceField
	tracer.Anchored = true
	tracer.CanCollide = false
	local distance = (plr.Character.Head.CFrame.p - pos).magnitude
	tracer.Size = Vector3.new(0.1, 0.1, distance)
	tracer.CFrame = CFrame.new(plr.Character.Head.CFrame.p, pos) * CFrame.new(0, 0, -distance / 2)
	game:GetService("Debris"):AddItem(tracer, 0.5)
end

function TPf()
	if thirdPerson then
		plr.CameraMaxZoomDistance = 10
		plr.CameraMinZoomDistance = 10
		if cam:FindFirstChild("Arms") then
			for i,v in next,cam.Arms:GetDescendants() do
				if v:IsA("BasePart") then
					v.Transparency = 1
				end
			end
		end
	else
		plr.CameraMaxZoomDistance = 0
		plr.CameraMinZoomDistance = 0
	end
end

function coolDown()
	shootCooldown = true
	if game.ReplicatedStorage.Weapons:FindFirstChild(tostring(client.gun)) then
		wait(game.ReplicatedStorage.Weapons:FindFirstChild(tostring(client.gun)).FireRate.Value)
		shootCooldown = false
	end
end

function fireBullet()
	if not shootCooldown then
		client.firebullet()
		coolDown()
	end
end

runserv.RenderStepped:connect(function()
	if rage and plr.Character and plr.Character:FindFirstChild("HumanoidRootPart") then
		for i,v in next,plrs:GetPlayers() do
			if v and v ~= plr and v.Character and v.Character:FindFirstChild("HumanoidRootPart") and plr.Character and plr.Character:FindFirstChild("HumanoidRootPart") and v.Team ~= plr.Team then
				local partHit, posHit, damageModifier, wallbang = simulateShot(v)
				if partHit and (damageModifier * client.gun.DMG.Value) > 5 then
					target = partHit
					fireBullet()
				else
					target = nil
				end
			end
		end
		if plr.Character and plr.Character.Humanoid then
			if antiAim == "Backward" then
				plr.Character.Humanoid.AutoRotate = false
				AA(180)
			else
				plr.Character.Humanoid.AutoRotate = true
			end
		end
	end
	if thirdPerson then
		TPf()
	end
end)

for i,v in next,plrs:GetPlayers() do
	v.CharacterAdded:connect(function(char)
		wait(1)
		if plrChams and visual and plr.Team ~= v.Team and v.Character and v ~= plr then
			chamPlr(v)
		end
	end)
end

plrs.ChildAdded:connect(function(v)
	v.CharacterAdded:connect(function(char)
		wait(1)
		if plrChams and visual and plr.Team ~= v.Team and v.Character and v ~= plr then
			chamPlr(v)
		end
	end)
end)

-- UILib
local lib = loadstring(game:HttpGet("https://pastebin.com/raw/T6sQMcLY", true))()

local rageTab = lib:Create("Rage")
local rageToggle = lib:AddToggle("Master Switch",function(state)
	rage = state
end)
local AA = lib:AddList("AntiAim",function(choice)
	antiAim = choice
end,{Choices = {"OFF","Backward"}})
local visTab = lib:Create("Visuals")
local visToggle = lib:AddToggle("Master Switch",function(state)
	visual = state
end)
local chams = lib:AddToggle("Chams",function(state)
	plrChams = state
	for i,v in next,plrs:GetPlayers() do
		if v.Team ~= plr.Team and v.Character and v.Character.HumanoidRootPart and v.Character.Humanoid.Health > 0 and plrChams then
			chamPlr(v)
		end
	end
end)
local BT = lib:AddToggle("Bullet Tracer",function(state)
	bulletTracer = state
end)
local TP = lib:AddToggle("ThirdPerson",function(state)
	thirdPerson = state
end)
-- Sellout Shit
local sellout1 = lib:Create("Credits")
local sellout2 = lib:Create("Terr#9198 | 393960375634100226")
local sellout3 = lib:Create("v3rm uid=1093603")

lib:build()
