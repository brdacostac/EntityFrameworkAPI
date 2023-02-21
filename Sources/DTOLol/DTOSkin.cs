using Model;

namespace DTOLol
{
    public class DTOSkin
    {
        public String Name
        {
            get; set;
        }

        public String Description
        {
            get; set;
        }

        public float Price
        {
            get; set;
        }

        public DTOChampion Champion
        {
            get; set;
        }

        public String Icon
        {
            get; set;
        }

        public String Image
        {
            get; set;
        }
    }
}