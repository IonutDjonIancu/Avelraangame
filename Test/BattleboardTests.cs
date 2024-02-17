namespace Tests;

[Collection("BattleboardTests")]
[Trait("Category", "BattleboardServiceTests")]
public class BattleboardTests : TestBase
{
    private const string PlayerName1 = "Test Player 1";

    [Fact(DisplayName = "Create battleboard should exist in snapshot.")]
    public void CreateBattleboardTest()
    {
        var actor = CreateBattleboardActor(PlayerName1);

        var board = _battleboard.CreateBattleboard(actor);

        board.Should().NotBeNull();
        board.GoodGuyPartyLeadId.Should().Be(actor.MainActor.Id);
        _snapshot.Battleboards.Count.Should().Be(1);
    }

    [Fact(DisplayName = "Getting the battleboards should return a list of all battleboards.")]
    public void GetBattleboardsTest()
    {
        _battleboard.CreateBattleboard(CreateBattleboardActor(PlayerName1));

        var boards = _battleboard.GetBattleboards();

        boards.Count.Should().Be(1);
    }

    [Fact(DisplayName = "Find battleboard should return the exact battleboard.")]
    public void FindBattleboardTest()
    {
        var actor = CreateBattleboardActor(PlayerName1);

        _battleboard.CreateBattleboard(actor);

        var character = TestUtils.GetCharacter(actor.MainActor.Id, actor.MainActor.PlayerId, _snapshot);

        var battleboard = _battleboard.FindBattleboard(character.Status.Gameplay.BattleboardId);

        battleboard.Should().NotBeNull();
    }

    [Fact(DisplayName = "Find character battleboard should return the exact battleboard.")]
    public void FindCharacterBattleboardTest()
    {
        var actor = CreateBattleboardActor(PlayerName1);

        _battleboard.CreateBattleboard(actor);

        var battleboard = _battleboard.FindCharacterBattleboard(actor);

        battleboard.Should().NotBeNull();
        battleboard.GetAllCharacters().Select(s => s.Identity.Id).ToList().Should().Contain(actor.MainActor.Id);
    }

    [Fact(DisplayName = "Joining a battleboard should be good or bad guy in battleboard.")]
    public void JoinBattleboardTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.WantsToBeGood = true;
        actor2.BattleboardId = board.Id;
        var actor2char = TestUtils.GetCharacter(actor2.MainActor.Id, actor2.MainActor.PlayerId, _snapshot);
        actor2char.Status.Worth = 1000;
        _battleboard.JoinBattleboard(actor2);

        var actor3 = CreateBattleboardActor("player 3");
        actor3.WantsToBeGood = false;
        actor3.BattleboardId = board.Id;
        _battleboard.JoinBattleboard(actor3);

        board.GoodGuys.Count.Should().Be(2);
        board.GoodGuyPartyLeadId.Should().Be(actor2.MainActor.Id);
        board.BadGuys.Count.Should().Be(1);
        board.BadGuyPartyLeadId.Should().Be(actor3.MainActor.Id);
    }

    [Fact(DisplayName = "Being kicked from a battleboard should remove character from battleboard.")]
    public void KickFromBattleboardTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.WantsToBeGood = true;
        actor2.BattleboardId = board.Id;
        var actor2char = TestUtils.GetCharacter(actor2.MainActor.Id, actor2.MainActor.PlayerId, _snapshot);
        actor2char.Status.Worth = 0;
        _battleboard.JoinBattleboard(actor2);

        board.GoodGuyPartyLeadId.Should().Be(actor1.MainActor.Id);

        actor1.TargetId = actor2.MainActor.Id;
        _battleboard.KickFromBattleboard(actor1);

        board.GoodGuys.Count.Should().Be(1);
        actor2char.Status.Gameplay.BattleboardId.Should().Be(string.Empty);
    }

    [Fact(DisplayName = "Trying to kick an enemy player from a battleboard should throw.")]
    public void KickEnemyFromBattleboardTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.WantsToBeGood = false;
        actor2.BattleboardId = board.Id;
        var actor2char = TestUtils.GetCharacter(actor2.MainActor.Id, actor2.MainActor.PlayerId, _snapshot);
        _battleboard.JoinBattleboard(actor2);

        actor1.TargetId = actor2.MainActor.Id;

        Assert.Throws<Exception>(() => _battleboard.KickFromBattleboard(actor1));
        actor2char.Status.Gameplay.BattleboardId.Should().Be(board.Id);
    }

    [Fact(DisplayName = "Kicking a player when you are both evil from a battleboard should remove character from battleboard.")]
    public void KickEvilFromBattleboardTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.WantsToBeGood = false;
        actor2.BattleboardId = board.Id;
        var actor2char = TestUtils.GetCharacter(actor2.MainActor.Id, actor2.MainActor.PlayerId, _snapshot);
        actor2char.Status.Worth = 1000;
        _battleboard.JoinBattleboard(actor2);

        var actor3 = CreateBattleboardActor("player 3");
        actor3.WantsToBeGood = false;
        actor3.BattleboardId = board.Id;
        _battleboard.JoinBattleboard(actor3);

        actor2.TargetId = actor3.MainActor.Id;

        _battleboard.KickFromBattleboard(actor2);

        board.BadGuys.Count.Should().Be(1);
        board.BadGuys.First().Identity.Id.Should().Be(actor2.MainActor.Id);
    }

    [Fact(DisplayName = "Only party leads can perform these actions.")]
    public void ActionRestrictedToPartyLeadsTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var actor1char = TestUtils.GetCharacter(actor1.MainActor.Id, actor1.MainActor.PlayerId, _snapshot);
        actor1char.Status.Worth = 1000;
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.WantsToBeGood = true;
        actor2.BattleboardId = board.Id;
        _battleboard.JoinBattleboard(actor2);

        var actor3 = CreateBattleboardActor("player 3");
        var actor3char = TestUtils.GetCharacter(actor3.MainActor.Id, actor3.MainActor.PlayerId, _snapshot);
        actor3char.Status.Worth = 1000;
        actor3.WantsToBeGood = false;
        actor3.BattleboardId = board.Id;
        _battleboard.JoinBattleboard(actor3);

        var actor4 = CreateBattleboardActor("player 4");
        actor4.WantsToBeGood = false;
        actor4.BattleboardId = board.Id;
        _battleboard.JoinBattleboard(actor4);

        actor4.TargetId = actor3.MainActor.Id;
        Assert.Throws<Exception>(() => _battleboard.KickFromBattleboard(actor4));
    }

    [Fact(DisplayName = "Leaving a battleboard should remove character from battleboard.")]
    public void LeaveBattleboardTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var actor1char = TestUtils.GetCharacter(actor1.MainActor.Id, actor1.MainActor.PlayerId, _snapshot);
        actor1char.Status.Worth = 1000;
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.WantsToBeGood = true;
        actor2.BattleboardId = board.Id;
        _battleboard.JoinBattleboard(actor2);

        var actor3 = CreateBattleboardActor("player 3");
        var actor3char = TestUtils.GetCharacter(actor3.MainActor.Id, actor3.MainActor.PlayerId, _snapshot);
        actor3char.Status.Worth = 1000;
        actor3.WantsToBeGood = false;
        actor3.BattleboardId = board.Id;
        _battleboard.JoinBattleboard(actor3);

        var actor4 = CreateBattleboardActor("player 4");
        actor4.WantsToBeGood = false;
        actor4.BattleboardId = board.Id;
        _battleboard.JoinBattleboard(actor4);

        _battleboard.LeaveBattleboard(actor2);
        board.GoodGuys.Count.Should().Be(1);
        board.GoodGuys.Select(s => s.Identity.Id).Should().NotContain(actor2.MainActor.Id);

        _battleboard.LeaveBattleboard(actor4);
        board.BadGuys.Count.Should().Be(1);
        board.BadGuys.Select(s => s.Identity.Id).Should().NotContain(actor4.MainActor.Id);

        _battleboard.LeaveBattleboard(actor1);
        board.GoodGuys.Count.Should().Be(0);
        board.GoodGuys.Select(s => s.Identity.Id).Should().NotContain(actor2.MainActor.Id);

        _battleboard.LeaveBattleboard(actor3);
        board.BadGuys.Count.Should().Be(0);
        board.BadGuys.Select(s => s.Identity.Id).Should().NotContain(actor3.MainActor.Id);

        _snapshot.Battleboards.Count.Should().Be(0);
    }

    [Fact(DisplayName = "Joining a battleboard brings your mercs with you.")]
    public void JoinBattleboardWithMercsTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var actor1char = TestUtils.GetCharacter(actor1.MainActor.Id, actor1.MainActor.PlayerId, _snapshot);
        actor1char.Status.Worth = 10000;
        actor1char.Status.Wealth = 10000;
        
        var location = _snapshot.Locations.Find(s => s.FullName == actor1char.Status.Position.GetPositionFullName())!;

        var hire = new CharacterHireMercenary
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = actor1.MainActor.Id,
                PlayerId = actor1.MainActor.PlayerId,
            },
            MercenaryId = location.Mercenaries.First().Identity.Id
        };

        _characters.HireMercenaryForCharacter(hire);

        var board = _battleboard.CreateBattleboard(actor1);
        board.GoodGuys.Select(s => s.Identity.Id).Should().Contain(actor1char.Mercenaries.First().Identity.Id);
    }

    [Fact(DisplayName = "Leaving a battleboard returns the mercs to town.")]
    public void LeaveBattleboardWithMercsTest()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var board = _battleboard.CreateBattleboard(actor1);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.BattleboardId = board.Id;
        actor2.WantsToBeGood = true;
        var actor2char = TestUtils.GetCharacter(actor2.MainActor.Id, actor2.MainActor.PlayerId, _snapshot);
        actor2char.Status.Worth = 10000;
        actor2char.Status.Wealth = 10000;

        var location = _snapshot.Locations.Find(s => s.FullName == actor2char.Status.Position.GetPositionFullName())!;

        var hire = new CharacterHireMercenary
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = actor2.MainActor.Id,
                PlayerId = actor2.MainActor.PlayerId,
            },
            MercenaryId = location.Mercenaries.First().Identity.Id
        };
        _characters.HireMercenaryForCharacter(hire);

        _battleboard.JoinBattleboard(actor2);
        board.GoodGuys.Select(s => s.Identity.Id).Should().Contain(actor2char.Mercenaries.First().Identity.Id);
        var actor2FirstMerc = actor2char.Mercenaries.First();

        _battleboard.LeaveBattleboard(actor2);
        location.Mercenaries.Find(m => m.Identity.Id == actor2FirstMerc.Identity.Id).Should().NotBeNull();
    }

    [Fact(DisplayName = "Starting combat should lock all characters on battleboard.")]
    public void BattleboardStartCombatTest()
    {
        var board = CreateBattleboardAndStartCombat();
        var location = _snapshot.Locations.Find(s => s.Name == board.GoodGuys.First().Status.Position.Location)!;

        board.GetAllCharacters().Count.Should().Be(6);
        board.IsInCombat.Should().BeTrue();
        board.CanLvlUp.Should().BeTrue();
        board.RoundNr.Should().Be(1);
        board.Quest.EffortLvl.Should().Be(location.Effort);
        board.GetAllCharacters().Select(s => s.Status.Gameplay.IsLocked).ToList().Should().NotContain(false);
        board.BattleOrder.Count.Should().Be(6);
        board.LastActionResult.Length.Should().BeGreaterThan(10);
    }

    [Fact(DisplayName = "Attacking in combat should display correctly.")]
    public void BattleboardAttackTest()
    {
        var board = CreateBattleboardAndStartCombat();

        var attacker = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.BattleOrder[0])!;
        Character defender;
        if (attacker.Status.Gameplay.IsGoodGuy)
        {
            defender = board.BadGuys.First();
        } 
        else
        {
            defender = board.GoodGuys.First();
        }

        attacker.Sheet.Skills.Melee = 1000;
        defender.Sheet.Skills.Melee = 10;
        var attackerInitialResolve = attacker.Sheet.Assets.ResolveLeft;
        var defenderInitialResolve = defender.Sheet.Assets.ResolveLeft;

        var actor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = attacker.Identity.Id,
                PlayerId = attacker.Identity.PlayerId
            },
            TargetId = defender.Identity.Id
        };
        _battleboard.Attack(actor);

        attacker.Sheet.Assets.ResolveLeft.Should().BeLessThan(attackerInitialResolve);
        defender.Sheet.Assets.ResolveLeft.Should().BeLessThan(defenderInitialResolve);
    }

    [Fact(DisplayName = "Casting offensive arcane in combat should display correctly.")]
    public void BattleboardCastTest()
    {
        var board = CreateBattleboardAndStartCombat();

        var attacker = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.BattleOrder[0])!;
        Character defender;
        if (attacker.Status.Gameplay.IsGoodGuy)
        {
            defender = board.BadGuys.First();
        }
        else
        {
            defender = board.GoodGuys.First();
        }

        attacker.Sheet.Skills.Arcane = 1000;
        attacker.Sheet.Assets.ManaLeft = 1000;
        var attackerInitialResolve = attacker.Sheet.Assets.ResolveLeft;
        var attackerInitialMana = attacker.Sheet.Assets.ManaLeft;
        var defenderInitialResolve = defender.Sheet.Assets.ResolveLeft;

        var actor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = attacker.Identity.Id,
                PlayerId = attacker.Identity.PlayerId
            },
            TargetId = defender.Identity.Id,
        };

        _battleboard.Cast(actor);

        attacker.Sheet.Assets.ResolveLeft.Should().BeLessThan(attackerInitialResolve);
        attacker.Sheet.Assets.ManaLeft.Should().BeLessThan(attackerInitialMana);
        defender.Sheet.Assets.ResolveLeft.Should().BeLessThan(defenderInitialResolve);
    }

    [Fact(DisplayName = "Casting offensive arcane in combat against a stronger spellcaster should not deal damage.")]
    public void BattleboardCastAgainstCasterTest()
    {
        var board = CreateBattleboardAndStartCombat();

        var attacker = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.BattleOrder[0])!;
        Character defender;
        if (attacker.Status.Gameplay.IsGoodGuy)
        {
            defender = board.BadGuys.First();
        }
        else
        {
            defender = board.GoodGuys.First();
        }

        attacker.Sheet.Skills.Arcane = 10;
        attacker.Sheet.Assets.ManaLeft = 1000;
        defender.Status.Traits.Class = CharactersLore.Classes.Mage;
        defender.Sheet.Skills.Arcane = 1000;
        var attackerInitialResolve = attacker.Sheet.Assets.ResolveLeft;
        var attackerInitialMana = attacker.Sheet.Assets.ManaLeft;
        var defenderInitialResolve = defender.Sheet.Assets.ResolveLeft;

        var actor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = attacker.Identity.Id,
                PlayerId = attacker.Identity.PlayerId
            },
            TargetId = defender.Identity.Id,
        };

        _battleboard.Cast(actor);

        attacker.Sheet.Assets.ResolveLeft.Should().BeLessThan(attackerInitialResolve);
        attacker.Sheet.Assets.ManaLeft.Should().BeLessThan(attackerInitialMana);
        defender.Sheet.Assets.ResolveLeft.Should().Be(defenderInitialResolve);
    }

    [Fact(DisplayName = "Mending characters should display correctly.")]
    public void BattleboardMendTest()
    {
        var board = CreateBattleboardAndStartCombat();

        var attacker = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.BattleOrder[0])!;
        Character defender;
        if (attacker.Status.Gameplay.IsGoodGuy)
        {
            defender = board.GoodGuys.First();
        }
        else
        {
            defender = board.BadGuys.First();
        }

        attacker.Sheet.Skills.Apothecary = 1000;
        var attackerInitialResolve = attacker.Sheet.Assets.ResolveLeft;
        defender.Sheet.Assets.ResolveLeft = 10;
        var defenderInitialResolve = defender.Sheet.Assets.ResolveLeft;

        var actor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = attacker.Identity.Id,
                PlayerId = attacker.Identity.PlayerId
            },
            TargetId = defender.Identity.Id,
        };

        _battleboard.Mend(actor);

        attacker.Sheet.Assets.ResolveLeft.Should().BeLessThanOrEqualTo(attackerInitialResolve);
        defender.Sheet.Assets.ResolveLeft.Should().BeGreaterThan(defenderInitialResolve);
    }

    [Fact(DisplayName = "Mending an enemy should throw.")]
    public void BattleboardMendEnemyTest()
    {
        var board = CreateBattleboardAndStartCombat();

        var attacker = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.BattleOrder[0])!;
        Character defender;
        if (attacker.Status.Gameplay.IsGoodGuy)
        {
            defender = board.BadGuys.First();
        }
        else
        {
            defender = board.GoodGuys.First();
        }

        attacker.Sheet.Skills.Apothecary = 1000;
        var attackerInitialResolve = attacker.Sheet.Assets.ResolveLeft;
        defender.Sheet.Assets.ResolveLeft = 10;
        var defenderInitialResolve = defender.Sheet.Assets.ResolveLeft;

        var actor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = attacker.Identity.Id,
                PlayerId = attacker.Identity.PlayerId
            },
            TargetId = defender.Identity.Id,
        };

        Assert.Throws<Exception>(() => _battleboard.Mend(actor));
    }

    [Fact(DisplayName = "Hiding should display correctly.")]
    public void BattleboardHideTest()
    {
        var board = CreateBattleboardAndStartCombat();

        var attacker = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.BattleOrder[0])!;

        attacker.Sheet.Skills.Hide = 100000;

        var actor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = attacker.Identity.Id,
                PlayerId = attacker.Identity.PlayerId
            },
        };

        _battleboard.Hide(actor);

        attacker.Status.Gameplay.IsHidden.Should().BeTrue();
    }

    [Fact(DisplayName = "Casting traps in combat should display correctly.")]
    public void BattleboardTrapsTest()
    {
        var board = CreateBattleboardAndStartCombat();

        var attacker = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.BattleOrder[0])!;
        var defenderTotalResPool = 0;
        if (attacker.Status.Gameplay.IsGoodGuy)
        {
            defenderTotalResPool = board.BadGuys.Select(s => s.Sheet.Assets.ResolveLeft).Sum();
        }
        else
        {
            defenderTotalResPool = board.GoodGuys.Select(s => s.Sheet.Assets.ResolveLeft).Sum();
        }

        attacker.Sheet.Skills.Traps = 100000;
        var attackerInitialResolve = attacker.Sheet.Assets.ResolveLeft;

        var actor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = attacker.Identity.Id,
                PlayerId = attacker.Identity.PlayerId
            },
        };

        _battleboard.Traps(actor);

        attacker.Sheet.Assets.ResolveLeft.Should().BeLessThan(attackerInitialResolve);
        if (attacker.Status.Gameplay.IsGoodGuy)
        {
            board.BadGuys.Select(s => s.Sheet.Assets.ResolveLeft).Sum().Should().BeLessThan(defenderTotalResPool);
        }
        else
        {
            board.GoodGuys.Select(s => s.Sheet.Assets.ResolveLeft).Sum().Should().BeLessThan(defenderTotalResPool);
        }
    }

    [Fact(DisplayName = "Resting in combat should display correctly.")]
    public void BattleboardRestTest()
    {
        var board = CreateBattleboardAndStartCombat();

        var attacker = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.BattleOrder[0])!;

        var initialResolveLeft = 10;
        var initialManaLeft = 10;
        attacker.Sheet.Assets.ResolveLeft = initialResolveLeft;
        attacker.Sheet.Assets.ManaLeft = initialManaLeft;

        var actor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = attacker.Identity.Id,
                PlayerId = attacker.Identity.PlayerId
            },
        };

        _battleboard.Rest(actor);

        var resolveToHeal = (int)((attacker.Sheet.Assets.Resolve - initialResolveLeft) * 0.1);
        attacker.Sheet.Assets.ResolveLeft.Should().Be(initialResolveLeft + resolveToHeal);

        var manaToHeal = (int)((attacker.Sheet.Assets.Mana - initialManaLeft) * 0.2);
        attacker.Sheet.Assets.ManaLeft.Should().Be(initialManaLeft + manaToHeal);
    }

    [Fact(DisplayName = "Let AiAct should display correctly.")]
    public void BattleboardLetAiActTest()
    {
        var board = CreateBattleboardAndStartCombat();

        var firstCharacter = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.BattleOrder[0])!;
        var actor1 = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = firstCharacter.Identity.Id,
                PlayerId = firstCharacter.Identity.PlayerId
            },
        };
        if (board.BattleOrder.First() == board.GoodGuyPartyLeadId || board.BattleOrder.First() == board.BadGuyPartyLeadId) _battleboard.Rest(actor1);

        var secondCharacter = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.BattleOrder[0])!;
        var actor2 = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = secondCharacter.Identity.Id,
                PlayerId = secondCharacter.Identity.PlayerId
            },
        };
        // twice to make sure we exclude both party leader possibilities
        if (board.BattleOrder.First() == board.GoodGuyPartyLeadId || board.BattleOrder.First() == board.BadGuyPartyLeadId) _battleboard.Rest(actor2);

        var npc = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.BattleOrder[0])!;
        npc.Identity.PlayerId = Guid.Empty.ToString();
        var npcActor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.GoodGuyPartyLeadId)!.Identity.Id,
                PlayerId = board.GetAllCharacters().FirstOrDefault(s => s.Identity.Id == board.GoodGuyPartyLeadId)!.Identity.PlayerId
            },
        };

        var totalEnemyResolve = 0;

        if (npc.Status.Gameplay.IsGoodGuy)
        {
            totalEnemyResolve = board.BadGuys.Select(s => s.Sheet.Assets.ResolveLeft).Sum();
        }
        else
        {
            totalEnemyResolve = board.GoodGuys.Select(s => s.Sheet.Assets.ResolveLeft).Sum();
        }

        _battleboard.LetAiAct(npcActor);

        if (npc.Status.Gameplay.IsGoodGuy)
        {
            board.BadGuys.Select(s => s.Sheet.Assets.ResolveLeft).Sum().Should().BeLessThanOrEqualTo(totalEnemyResolve);
        }
        else
        {
            board.GoodGuys.Select(s => s.Sheet.Assets.ResolveLeft).Sum().Should().BeLessThanOrEqualTo(totalEnemyResolve);
        }
    }


    [Fact(DisplayName = "End round should increment battleboard round nr correctly.")]
    public void BattleboardEndRoundTest()
    {
        var board = CreateBattleboardAndStartCombat();

        var attacker = board.GetAllCharacters().Find(s => s.Identity.Id == board.GoodGuyPartyLeadId)!;

        var actor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = attacker.Identity.Id,
                PlayerId = attacker.Identity.PlayerId
            },
        };

        board.BattleOrder.Clear();

        _battleboard.EndRound(actor);

        board.RoundNr.Should().BeGreaterThan(1);
    }

    [Fact(DisplayName = "End combat should distribute the items to the party leader.")]
    public void BattleboardEndCombatTest()
    {
        var board = CreateBattleboardAndStartCombat();

        var partyLead = board.GetAllCharacters().Find(s => s.Identity.Id == board.GoodGuyPartyLeadId)!;

        var actor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = partyLead.Identity.Id,
                PlayerId = partyLead.Identity.PlayerId
            },
        };

        var anItem = board.BadGuys.SelectMany(s => s.Inventory.GetAllEquipedItems()).ToList().First();

        board.BadGuys.ForEach(s => s.Status.Gameplay.IsAlive = false);

        _battleboard.EndCombat(actor);

        partyLead.Inventory.Supplies.Select(s => s.Identity.Id).Should().Contain(anItem.Identity.Id);
    }

    [Fact(DisplayName = "Make camp should replenish mana and resolve to full.")]
    public void BattleboardMakeCampTest()
    {
        var board = CreateBattleboardAndStartCombat();

        board.BadGuys.Clear();
        board.BadGuyPartyLeadId = string.Empty;

        var partyLead = board.GetAllCharacters().Find(s => s.Identity.Id == board.GoodGuyPartyLeadId)!;

        var actor = new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = partyLead.Identity.Id,
                PlayerId = partyLead.Identity.PlayerId
            },
        };

        board.GetAllCharacters().ForEach(s => 
        {
            s.Sheet.Assets.ResolveLeft = (int)(s.Sheet.Assets.ResolveLeft * 0.5);
            s.Sheet.Assets.ManaLeft = (int)(s.Sheet.Assets.ManaLeft * 0.5);
        });

        _battleboard.EndCombat(actor);

        _battleboard.MakeCamp(actor);

        var fullResolve = board.GetAllCharacters().Select(s => s.Sheet.Assets.Resolve).Sum();
        board.GetAllCharacters().Select(s => s.Sheet.Assets.ResolveLeft).Sum().Should().Be(fullResolve);

        var fullMana = board.GetAllCharacters().Select(s => s.Sheet.Assets.Mana).Sum();
        board.GetAllCharacters().Select(s => s.Sheet.Assets.ManaLeft).Sum().Should().Be(fullMana);
    }

    [Fact(DisplayName = "Start repeatable quest should reflect on the battleboard.")]
    public void BattleboardStartQuestTest()
    {
        var actor = CreateBattleboardActor(PlayerName1);
        var actorCharacter = TestUtils.GetCharacter(actor.MainActor.Id, actor.MainActor.PlayerId, _snapshot);
        var location = _snapshot.Locations.Find(s => s.FullName == actorCharacter.Status.Position.GetPositionFullName())!;

        var board = _battleboard.CreateBattleboard(actor);

        var questId = location.Quests.First(s => s.IsRepeatable).Id;
        actor.QuestId = questId;    

        board = _battleboard.SelectQuest(actor);

        board.Quest.Id.Should().Be(questId);    
    }

    [Fact(DisplayName = "Start unique quest should remove it from location quests.")]
    public void BattleboardStartUniqueQuestTest()
    {
        var actor = CreateBattleboardActor(PlayerName1);
        var actorCharacter = TestUtils.GetCharacter(actor.MainActor.Id, actor.MainActor.PlayerId, _snapshot);
        var location = _snapshot.Locations.Find(s => s.FullName == actorCharacter.Status.Position.GetPositionFullName())!;

        var board = _battleboard.CreateBattleboard(actor);

        var questId = location.Quests.First(s => s.IsRepeatable == false).Id;
        actor.QuestId = questId;

        board = _battleboard.SelectQuest(actor);

        board.Quest.Id.Should().Be(questId);
        location.Quests.Should().NotContain(s => s.Id == questId);
    }

    [Fact(DisplayName = "Abandon quest should return mercs to location.")]
    public void BattleboardAbandonQuestReturnsMercsTest()
    {
        //throw new NotImplementedException();
    }

    #region private methods
    private Battleboard CreateBattleboardAndStartCombat()
    {
        var (board, mainActor) = CreateBattleboardWithCombatants();

        _battleboard.StartCombat(mainActor);

        return board;
    }

    private (Battleboard board, BattleboardActor mainActor) CreateBattleboardWithCombatants()
    {
        var actor1 = CreateBattleboardActor(PlayerName1);
        var actor1char = TestUtils.GetCharacter(actor1.MainActor.Id, actor1.MainActor.PlayerId, _snapshot);
        actor1char.Status.Worth = 10000;
        actor1char.Status.Wealth = 10000;

        var board = _battleboard.CreateBattleboard(actor1);

        var location = _snapshot.Locations.Find(s => s.FullName == actor1char.Status.Position.GetPositionFullName())!;
        var merc1 = _npcs.GenerateGoodGuy(location.Name);
        var merc2 = _npcs.GenerateGoodGuy(location.Name);
        location.Mercenaries.Clear();
        location.Mercenaries.Add(merc1);
        location.Mercenaries.Add(merc2);

        var actor2 = CreateBattleboardActor("player 2");
        actor2.BattleboardId = board.Id;
        actor2.WantsToBeGood = true;
        var actor2char = TestUtils.GetCharacter(actor2.MainActor.Id, actor2.MainActor.PlayerId, _snapshot);
        actor2char.Status.Wealth = 10000;

        var hireFor2 = new CharacterHireMercenary
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = actor2.MainActor.Id,
                PlayerId = actor2.MainActor.PlayerId,
            },
            MercenaryId = location.Mercenaries.First().Identity.Id
        };
        _characters.HireMercenaryForCharacter(hireFor2);
        _battleboard.JoinBattleboard(actor2);

        var actor3 = CreateBattleboardActor("player 3");
        actor3.BattleboardId = board.Id;
        actor3.WantsToBeGood = false;
        var actor3char = TestUtils.GetCharacter(actor3.MainActor.Id, actor3.MainActor.PlayerId, _snapshot);
        actor3char.Status.Worth = 10000;
        actor3char.Status.Wealth = 10000;
        _battleboard.JoinBattleboard(actor3);

        var actor4 = CreateBattleboardActor("player 4");
        actor4.BattleboardId = board.Id;
        actor4.WantsToBeGood = false;
        var actor4char = TestUtils.GetCharacter(actor4.MainActor.Id, actor4.MainActor.PlayerId, _snapshot);
        actor4char.Status.Wealth = 10000;

        var hireFor4 = new CharacterHireMercenary
        {
            CharacterIdentity = new CharacterIdentity
            {
                Id = actor4.MainActor.Id,
                PlayerId = actor4.MainActor.PlayerId,
            },
            MercenaryId = location.Mercenaries.First().Identity.Id
        };
        _characters.HireMercenaryForCharacter(hireFor4);
        _battleboard.JoinBattleboard(actor4);

        return (board, actor1);
    }

    private BattleboardActor CreateBattleboardActor(string playerName)
    {
        var character = TestUtils.CreateAndGetCharacter(playerName, _players, _characters, _snapshot);

        return new BattleboardActor
        {
            MainActor = new CharacterIdentity
            {
                Id = character.Identity.Id,
                PlayerId = character.Identity.PlayerId,
            },
        };
    }
    #endregion
}
