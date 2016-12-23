using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tochas.FuzzyLogic;
using Tochas.FuzzyLogic.MembershipFunctions;
using Tochas.FuzzyLogic.Expressions;

namespace UnitTest
{
    [TestClass]
    public class FuzzyLogicTests
    {


        public enum Temperature { Cold =0, Warm, Hot }
        public enum Command { Cool =0, DoNothing, Heat}

        [TestMethod]
        public void EnumEquality()
        {
            Assert.IsFalse(Temperature.Cold.Equals(Command.Cool));
            Assert.IsTrue(Temperature.Cold.Equals(Temperature.Cold));
            Assert.IsFalse(Temperature.Cold.Equals(Temperature.Hot));
        }

        [TestMethod]
        public void TestFuzzySet_Simple()
        {
            FuzzyValueSet set = new FuzzyValueSet();
            FuzzyValue<Temperature> value = new FuzzyValue<Temperature>() { linguisticVariable = Temperature.Hot, membershipDegree = 0.5f };
            set.Set(value);
            var result = set.Get(Temperature.Hot);
            Assert.AreEqual(Temperature.Hot, result.linguisticVariable);
            Assert.AreEqual(0.5f, result.membershipDegree);
        }

        [TestMethod]
        public void TestFuzzySet_SameEnum()
        {
            FuzzyValueSet set = new FuzzyValueSet();
            FuzzyValue<Temperature> value01 = new FuzzyValue<Temperature>() { linguisticVariable = Temperature.Hot, membershipDegree = 0.5f };
            FuzzyValue<Temperature> value02 = new FuzzyValue<Temperature>() { linguisticVariable = Temperature.Warm, membershipDegree = 1f };
            set.Set(value02);
            set.Set(value01);
            var result = set.Get(Temperature.Hot);
            Assert.AreEqual(Temperature.Hot, result.linguisticVariable);
            Assert.AreEqual(0.5f, result.membershipDegree);
        }

        [TestMethod]
        public void TestFuzzySet_SameEnum_SameVar()
        {
            FuzzyValueSet set = new FuzzyValueSet();
            FuzzyValue<Temperature> value01 = new FuzzyValue<Temperature>() { linguisticVariable = Temperature.Hot, membershipDegree = 0.5f };
            FuzzyValue<Temperature> value02 = new FuzzyValue<Temperature>() { linguisticVariable = Temperature.Hot, membershipDegree = 1f };
            set.Set(value02);
            set.Set(value01);
            var result = set.Get(Temperature.Hot);
            Assert.AreEqual(Temperature.Hot, result.linguisticVariable);
            Assert.AreEqual(0.5f, result.membershipDegree);
        }

        [TestMethod]
        public void TestFuzzySet_DiffEnum_A()
        {
            FuzzyValueSet set = new FuzzyValueSet();
            FuzzyValue<Temperature> value01 = new FuzzyValue<Temperature>() { linguisticVariable = Temperature.Hot, membershipDegree = 0.5f };
            FuzzyValue<Command> value02 = new FuzzyValue<Command>() { linguisticVariable = Command.Heat, membershipDegree = 1f };
            set.Set(value02);
            set.Set(value01);
            var result = set.Get(Temperature.Hot);
            Assert.AreEqual(Temperature.Hot, result.linguisticVariable);
            Assert.AreEqual(0.5f, result.membershipDegree);
        }

        [TestMethod]
        public void TestFuzzySet_DiffEnum_B()
        {
            FuzzyValueSet set = new FuzzyValueSet();
            FuzzyValue<Temperature> value01 = new FuzzyValue<Temperature>() { linguisticVariable = Temperature.Hot, membershipDegree = 0.5f };
            FuzzyValue<Command> value02 = new FuzzyValue<Command>() { linguisticVariable = Command.Heat, membershipDegree = 1f };
            set.Set(value02);
            set.Set(value01);
            var result = set.Get(Command.Heat);
            Assert.AreEqual(Command.Heat, result.linguisticVariable);
            Assert.AreEqual(1f, result.membershipDegree);
        }


        [TestMethod]
        public void TestLerp()
        {
            Coords c0 = new Coords(0f, 0f);
            Coords c1 = new Coords(1f, 0.5f);

            float result = Coords.Lerp(c0, c1, 2.0f);
            Assert.AreEqual(1.0f, result);
            result = Coords.Lerp(c0, c1, 1.0f);
            Assert.AreEqual(0.5f, result);
            result = Coords.Lerp(c0, c1, 0.5f);
            Assert.AreEqual(0.25f, result);
            result = Coords.Lerp(c0, c1, -10.0f);
            Assert.AreEqual(-5.0f, result);
        }

        [TestMethod]
        public void TestTriangularMemebershipFunction()
        {
            Coords c0 = new Coords(2.5f, 0f);
            Coords c1 = new Coords(5f, 1f);
            Coords c2 = new Coords(7.5f, 0.25f);
            TriangularMemebershipFunction membFunc = new TriangularMemebershipFunction(c1, c0, c2);
            Assert.AreEqual(2.5f ,membFunc.P0.X);
            Assert.AreEqual(5f, membFunc.P1.X);
            Assert.AreEqual(7.5f, membFunc.P2.X);
            var result = membFunc.fX(0.0f);
            Assert.AreEqual(0f, result);
            result = membFunc.fX(3.75f);
            Assert.AreEqual(0.5f, result);
            result = membFunc.fX(5.0f);
            Assert.AreEqual(1f, result);
            result = membFunc.fX(6.25f);
            Assert.AreEqual(0.625f, result);
            result = membFunc.fX(10.0f);
            Assert.AreEqual(0.25f, result);
        }

        [TestMethod]
        public void FuzzyAnd_01()
        {
            FuzzyValue<Temperature> val01 = new FuzzyValue<Temperature>(Temperature.Cold, 0.8f);
            FuzzyValue<Temperature> val02 = new FuzzyValue<Temperature>(Temperature.Warm, 0.25f);
            FuzzyValueSet set = new FuzzyValueSet();
            set.Set(val01);
            set.Set(val02);
            FuzzyAnd exp = new FuzzyAnd(new FuzzyVariableExpression<Temperature>(Temperature.Cold), new FuzzyVariableExpression<Temperature>(Temperature.Warm));
            var result = exp.Evaluate(set);
            Assert.AreEqual(0.25f, result);
        }

        [TestMethod]
        public void FuzzyOr_01()
        {
            FuzzyValue<Temperature> val01 = new FuzzyValue<Temperature>(Temperature.Cold, 0.8f);
            FuzzyValue<Temperature> val02 = new FuzzyValue<Temperature>(Temperature.Warm, 0.25f);
            FuzzyValueSet set = new FuzzyValueSet();
            set.Set(val01);
            set.Set(val02);
            FuzzyOr exp = new FuzzyOr(new FuzzyVariableExpression<Temperature>(Temperature.Cold), new FuzzyVariableExpression<Temperature>(Temperature.Warm));
            var result = exp.Evaluate(set);
            Assert.AreEqual(0.8f, result);
        }

        [TestMethod]
        public void FuzzyNot_01()
        {
            FuzzyValue<Temperature> val01 = new FuzzyValue<Temperature>(Temperature.Cold, 0.8f);
            FuzzyValue<Temperature> val02 = new FuzzyValue<Temperature>(Temperature.Warm, 0.25f);
            FuzzyValueSet set = new FuzzyValueSet();
            set.Set(val01);
            set.Set(val02);
            FuzzyNot exp = new FuzzyNot(new FuzzyVariableExpression<Temperature>(Temperature.Cold));
            var result = exp.Evaluate(set);
            Assert.AreEqual(0.2f, result, 0.000005);
        }

        [TestMethod]
        public void FuzzyExpressionBuilder()
        {
            Coords c0 = new Coords(2.5f, 0f);
            Coords c1 = new Coords(5f, 1f);
            Coords c2 = new Coords(7.5f, 0.25f);
            TriangularMemebershipFunction membFunc = new TriangularMemebershipFunction(c1, c0, c2);
            FuzzyVariable<Temperature> cold = new FuzzyVariable<Temperature>(Temperature.Cold, membFunc);
            FuzzyVariable<Temperature> warm = new FuzzyVariable<Temperature>(Temperature.Warm, membFunc);
            FuzzyVariable<Temperature> hot = new FuzzyVariable<Temperature>(Temperature.Hot, membFunc);
            var exp = cold.AsExpression().And(warm).Or(hot.AsExpression().Not());
            string expected = "((Cold AND Warm) OR NOT(Hot))";
            Assert.IsNotNull(exp);
            Assert.AreEqual(expected, exp.ToString());
        }

        public void GetMembershipFunctions(out IMemebershipFunction cold, out IMemebershipFunction warm, out IMemebershipFunction hot)
        {
            Coords c0;
            Coords c1;
            Coords c2;

            c0.X = -270;
            c0.Y = 1;
            c1.X = 5;
            c1.Y = 1;
            c2.X = 20;
            c2.Y = 0;
            cold = new TriangularMemebershipFunction(c0, c1, c2);

            c0.X = 5;
            c0.Y = 0;
            c1.X = 20;
            c1.Y = 1;
            c2.X = 30;
            c2.Y = 0;
            warm = new TriangularMemebershipFunction(c0, c1, c2);

            c0.X = 20;
            c0.Y = 0;
            c1.X = 30;
            c1.Y = 1;
            c2.X = 100;
            c2.Y = 1;
            hot = new TriangularMemebershipFunction(c0, c1, c2);
        }

       [TestMethod]
       public void TestFuzzyfication()
        {
            IMemebershipFunction coldFx, warmFx, hotFx;
            this.GetMembershipFunctions(out coldFx, out warmFx, out hotFx);

            FuzzyVariable<Temperature> cold = new FuzzyVariable<Temperature>(Temperature.Cold, coldFx);
            FuzzyVariable<Temperature> warm = new FuzzyVariable<Temperature>(Temperature.Warm, warmFx);
            FuzzyVariable<Temperature> hot = new FuzzyVariable<Temperature>(Temperature.Hot, hotFx);

            float inputTemp = 0;

            var coldVal = cold.fX(inputTemp);
            var warmVal = warm.fX(inputTemp);
            var hotVal = hot.fX(inputTemp);

            Assert.AreEqual(1.0f, coldVal.membershipDegree);
            Assert.AreEqual(0.0f, warmVal.membershipDegree);
            Assert.AreEqual(0.0f, hotVal.membershipDegree);

            inputTemp = 30;
            coldVal = cold.fX(inputTemp);
            warmVal = warm.fX(inputTemp);
            hotVal = hot.fX(inputTemp);

            Assert.AreEqual(0.0f, coldVal.membershipDegree, 0.000005f);
            Assert.AreEqual(0.0f, warmVal.membershipDegree, 0.000005f);
            Assert.AreEqual(1.0f, hotVal.membershipDegree, 0.000005f);

            inputTemp = 10;
            coldVal = cold.fX(inputTemp);
            warmVal = warm.fX(inputTemp);
            hotVal = hot.fX(inputTemp);

            Assert.AreEqual(0.6666666667f, coldVal.membershipDegree, 0.000005f);
            Assert.AreEqual(0.3333333333f, warmVal.membershipDegree, 0.000005f);
            Assert.AreEqual(0.0f, hotVal.membershipDegree, 0.000005f);
        }

        [TestMethod]
        public void TestVariablesRepresentativeValues()
        {

        }

        [TestMethod]
        public void TestFuzzyRule()
        {
            IMemebershipFunction coldFx, warmFx, hotFx;
            this.GetMembershipFunctions(out coldFx, out warmFx, out hotFx);
            
            FuzzyVariable<Temperature> cold = new FuzzyVariable<Temperature>(Temperature.Cold, coldFx);
            FuzzyVariable<Temperature> warm = new FuzzyVariable<Temperature>(Temperature.Warm, warmFx);
            FuzzyVariable<Temperature> hot = new FuzzyVariable<Temperature>(Temperature.Hot, hotFx);

            var exp = cold.AsExpression().And(warm).Or(hot.AsExpression().Not());
            FuzzyRule<Command> rule = new FuzzyRule<Command>(Command.Heat, exp);

            float inputTemp = 10;        
            FuzzyValueSet valueSet = new FuzzyValueSet();
            valueSet.Set(cold.fX(inputTemp));
            valueSet.Set(warm.fX(inputTemp));
            valueSet.Set(hot.fX(inputTemp));
            // 0.6666 AND 0.3333 OR NOT 0.0

            var result = rule.Evaluate(valueSet);
            Assert.AreEqual(1.0f, result.membershipDegree, 0.000005f);
        }
    }
}
