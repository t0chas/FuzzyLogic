using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tochas.FuzzyLogic;
using Tochas.FuzzyLogic.MembershipFunctions;
using System.Collections.Generic;
using Tochas.FuzzyLogic.Evaluators;
using Tochas.FuzzyLogic.Mergers;
using Tochas.FuzzyLogic.Defuzzers;
using Tochas.FuzzyLogic.Expressions;

namespace UnitTest
{
    [TestClass]
    public class WeaponsExample
    {
        enum Desirability { Undesirable, Desirable, VeryDesirable }
        enum DistanceToTarget { Close, Medium, Far }
        enum AmmoStatus { Low, Okay, Loads }

        private FuzzySet<Desirability> GetDesirabilitySet()
        {
            IMemebershipFunction undesirableFx = new ShoulderMembershipFunction(0f, new Coords(25f, 1f), new Coords(50f, 0f), 100f);
            IMemebershipFunction desirableFx = new TriangularMemebershipFunction(new Coords(25f, 0f), new Coords(50f, 1f), new Coords(75f, 0f));
            IMemebershipFunction veryDesirableFx = new ShoulderMembershipFunction(0f, new Coords(50f, 0f), new Coords(75f, 1f), 100f);

            FuzzySet<Desirability> set = new FuzzySet<Desirability>();
            set.Set(new FuzzyVariable<Desirability>(Desirability.Undesirable, undesirableFx));
            set.Set(new FuzzyVariable<Desirability>(Desirability.Desirable, desirableFx));
            set.Set(new FuzzyVariable<Desirability>(Desirability.VeryDesirable, veryDesirableFx));
            return set;
        }

        private FuzzySet<DistanceToTarget> GetDistanceToTargetSet()
        {
            IMemebershipFunction closeFx = new ShoulderMembershipFunction(0f, new Coords(25f, 1f), new Coords(150f, 0f), 400f);
            IMemebershipFunction mediumFx = new TriangularMemebershipFunction(new Coords(25f, 0f), new Coords(150f, 1f), new Coords(300f, 0f));
            IMemebershipFunction farFx = new ShoulderMembershipFunction(0f, new Coords(150f, 0f), new Coords(300f, 1f), 400f);

            FuzzySet<DistanceToTarget> set = new FuzzySet<DistanceToTarget>();
            set.Set(DistanceToTarget.Close, closeFx);
            set.Set(DistanceToTarget.Medium, mediumFx);
            set.Set(DistanceToTarget.Far, farFx);
            return set;
        }

        private FuzzySet<AmmoStatus> GetAmmoStatusSet()
        {
            IMemebershipFunction lowFx = new ShoulderMembershipFunction(0f, new Coords(0f, 1f), new Coords(10f, 0f), 40f);
            IMemebershipFunction okayFx = new TriangularMemebershipFunction(new Coords(0f, 0f), new Coords(10f, 1f), new Coords(30f, 0f));
            IMemebershipFunction loadsFx = new ShoulderMembershipFunction(0f, new Coords(10f, 0f), new Coords(30f, 1f), 40f);

            FuzzySet<AmmoStatus> set = new FuzzySet<AmmoStatus>();
            set.Set(AmmoStatus.Low, lowFx);
            set.Set(AmmoStatus.Okay, okayFx);
            set.Set(AmmoStatus.Loads, loadsFx);
            return set;
        }

        private FuzzyRule<Desirability>[] GetRocketLauncherRules()
        {
            FuzzyRule<Desirability>[] rules = new FuzzyRule<Desirability>[9];
            rules[0] = DistanceToTarget.Far.Expr().And(AmmoStatus.Loads.Expr()).Then(Desirability.Desirable);
            rules[1] = DistanceToTarget.Far.Expr().And(AmmoStatus.Okay.Expr()).Then(Desirability.Undesirable);
            rules[2] = DistanceToTarget.Far.Expr().And(AmmoStatus.Low.Expr()).Then(Desirability.Undesirable);
            rules[3] = DistanceToTarget.Medium.Expr().And(AmmoStatus.Loads.Expr()).Then(Desirability.VeryDesirable);
            rules[4] = DistanceToTarget.Medium.Expr().And(AmmoStatus.Okay.Expr()).Then(Desirability.VeryDesirable);
            rules[5] = DistanceToTarget.Medium.Expr().And(AmmoStatus.Low.Expr()).Then(Desirability.Desirable);
            rules[6] = DistanceToTarget.Close.Expr().And(AmmoStatus.Loads.Expr()).Then(Desirability.Undesirable);
            rules[7] = DistanceToTarget.Close.Expr().And(AmmoStatus.Okay.Expr()).Then(Desirability.Undesirable);
            rules[8] = DistanceToTarget.Close.Expr().And(AmmoStatus.Low.Expr()).Then(Desirability.Undesirable);
            return rules;
        }

        private FuzzyRuleSet<Desirability> GetRocketLauncherRuleSet(FuzzySet<Desirability> desirability)
        {
            var rules = this.GetRocketLauncherRules();
            return new FuzzyRuleSet<Desirability>(desirability, rules);
        }

        [TestMethod]
        public void SetsAreValid()
        {
            var desirability = this.GetDesirabilitySet();
            var distance = this.GetDistanceToTargetSet();
            var ammo = this.GetAmmoStatusSet();
            Assert.IsTrue(desirability.IsValid());
            Assert.IsTrue(distance.IsValid());
            Assert.IsTrue(ammo.IsValid());
        }

        [TestMethod]
        public void RulesAreValid()
        {
            string[] expectedRules = new string[9];
            expectedRules[0] = "IF (Far AND Loads) THEN Desirable";
            var rules = this.GetRocketLauncherRules();
            Assert.AreEqual(expectedRules[0], rules[0].ToString());
        }

        [TestMethod]
        public void TestRuleEvaluation()
        {
            var desirability = this.GetDesirabilitySet();
            var distance = this.GetDistanceToTargetSet();
            var ammo = this.GetAmmoStatusSet();
            var rocketLaucherRules = this.GetRocketLauncherRules();

            FuzzyValueSet inputs = new FuzzyValueSet();
            distance.Evaluate(200f, inputs);
            ammo.Evaluate(8f, inputs);

            IRuleEvaluator<Desirability> ruleEvaluator = new RuleEvaluator<Desirability>();
            var result = ruleEvaluator.EvaluateRules(new List<FuzzyRule<Desirability>>(rocketLaucherRules), inputs);
            this.AssertFuzzyValue<Desirability>(Desirability.Desirable, 0.0f, result[0]);
            this.AssertFuzzyValue<Desirability>(Desirability.Undesirable, 0.33f, result[1]);
            this.AssertFuzzyValue<Desirability>(Desirability.Undesirable, 0.2f, result[2]);

            this.AssertFuzzyValue<Desirability>(Desirability.VeryDesirable, 0.0f, result[3]);
            this.AssertFuzzyValue<Desirability>(Desirability.VeryDesirable, 0.67f, result[4]);
            this.AssertFuzzyValue<Desirability>(Desirability.Desirable, 0.2f, result[5]);

            this.AssertFuzzyValue<Desirability>(Desirability.Undesirable, 0.0f, result[6]);
            this.AssertFuzzyValue<Desirability>(Desirability.Undesirable, 0.0f, result[7]);
            this.AssertFuzzyValue<Desirability>(Desirability.Undesirable, 0.0f, result[8]);
        }

        private void AssertFuzzyValue<T>(T linguisticVar, float memebershipDegree, FuzzyValue<T> fuzzyValue) where T : struct, IConvertible
        {
            Assert.AreEqual(linguisticVar, fuzzyValue.linguisticVariable);
            Assert.AreEqual(memebershipDegree, fuzzyValue.membershipDegree, 0.005f);
        }

        [TestMethod]
        public void TestValueMergeByOr()
        {
            var desirability = this.GetDesirabilitySet();
            var distance = this.GetDistanceToTargetSet();
            var ammo = this.GetAmmoStatusSet();
            var rocketLaucherRules = this.GetRocketLauncherRules();

            FuzzyValueSet inputs = new FuzzyValueSet();
            distance.Evaluate(200f, inputs);
            ammo.Evaluate(8f, inputs);

            IRuleEvaluator<Desirability> ruleEvaluator = new RuleEvaluator<Desirability>();
            var ruleOutputs = ruleEvaluator.EvaluateRules(new List<FuzzyRule<Desirability>>(rocketLaucherRules), inputs);

            IFuzzyValuesMerger<Desirability> merger = new CachedOutputsFuzzyValuesMerger<Desirability>();
            FuzzyValueSet mergedValues = new FuzzyValueSet();
            merger.MergeValues(ruleOutputs, mergedValues);
            this.AssertFuzzyValue<Desirability>(Desirability.Undesirable, 0.33f, mergedValues.GetValue(Desirability.Undesirable));
            this.AssertFuzzyValue<Desirability>(Desirability.Desirable, 0.2f, mergedValues.GetValue(Desirability.Desirable));
            this.AssertFuzzyValue<Desirability>(Desirability.VeryDesirable, 0.67f, mergedValues.GetValue(Desirability.VeryDesirable));
        }

        [TestMethod]
        public void TestDesirablityVarsRepresentativeValues()
        {
            var desirability = this.GetDesirabilitySet();
            var undesirable = desirability.Get(Desirability.Undesirable);
            var desirable = desirability.Get(Desirability.Desirable);
            var veryDesirable = desirability.Get(Desirability.VeryDesirable);
            Assert.AreEqual(12.5f, undesirable.MembershipFunction.RepresentativeValue, 0.005f);
            Assert.AreEqual(50f, desirable.MembershipFunction.RepresentativeValue, 0.005f);
            Assert.AreEqual(87.5f, veryDesirable.MembershipFunction.RepresentativeValue, 0.005f);
        }

        [TestMethod]
        public void TestMaxAvDefuzzer()
        {
            var desirability = this.GetDesirabilitySet();
            FuzzyValueSet mergedValues = new FuzzyValueSet();
            mergedValues.SetValue(new FuzzyValue<Desirability>(Desirability.Undesirable, 0.33f));
            mergedValues.SetValue(new FuzzyValue<Desirability>(Desirability.Desirable, 0.2f));
            mergedValues.SetValue(new FuzzyValue<Desirability>(Desirability.VeryDesirable, 0.67f));

            IDefuzzer<Desirability> defuzzer = new MaxAvDefuzzer<Desirability>();
            var result = defuzzer.Defuzze(desirability, mergedValues);
            Assert.AreEqual(60.625f, result, 0.0005f);
        }

        [TestMethod]
        public void TestInference()
        {
            var desirability = this.GetDesirabilitySet();
            var distance = this.GetDistanceToTargetSet();
            var ammo = this.GetAmmoStatusSet();
            var rocketRuleSet = this.GetRocketLauncherRuleSet(desirability);
            FuzzyValueSet inputs = new FuzzyValueSet();
            distance.Evaluate(200f, inputs);
            ammo.Evaluate(8f, inputs);
            var result = rocketRuleSet.Evaluate(inputs);
            Assert.AreEqual(60.625f, result, 0.25f);
        }
    }
}
