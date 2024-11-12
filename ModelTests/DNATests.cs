using Model.Genetics;

namespace ModelTests
{
    public class DNATests : DNA
    {
        public DNATests() : base(3)
        {
            
        }

        [Fact]
        public void CanAddGene()
        {
            Assert.Equal("B(000)", Code);

            addGene(3, 'X');

            Assert.Equal("B(0X(000)0)", Code);

            addGene(7, 'Y');

            Assert.Equal("B(0X(00Y(000))0)", Code);
        }

        [Fact]
        public void CanRemoveGene()
        {
            addGene(3, 'X');
            addGene(7, 'Y');
            Assert.Equal("B(0X(00Y(000))0)", Code);

            try { removeGene(3); }
            catch (Exception e) { }
            Assert.Equal("B(0X(00Y(000))0)", Code);//TODO move into own test

            removeGene(7);
            Assert.Equal("B(0X(000)0)", Code);

            removeGene(3);
            Assert.Equal("B(000)", Code);
        }

        [Fact]
        public void CanSwapGene()
        {
            addGene(3, 'X');
            addGene(7, 'Y');
            Assert.Equal("B(0X(00Y(000))0)", Code);

            swapGene(3, 'Z');
            Assert.Equal("B(0Z(00Y(000))0)", Code);
        }
    }
}