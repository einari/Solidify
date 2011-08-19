namespace Solidify
{
    public static class Stores
    {
        public static Store KomplettNo = new Store(310,"10","B2C","NO","NOK", "N");
        public static Store Norek = new Store(311,"11","B2B","NO","NOK", "N");
        public static Store KomplettSe = new Store(312,"12","B2C","SE","SEK", "V");
        public static Store NorekSe = new Store(318,"18","B2B","SE","SEK", "V");
        public static Store KomplettDk = new Store(321,"21","B2C","DK","DKK", "K");
        public static Store Inwarehouse = new Store(323,"23","B2C","SE","SEK", "V");
        public static Store Mpx = new Store(324,"10","B2C","NO","NOK", "N");
        public static Store Itegra = new Store(325,"25","B2B","NO","NOK", "N");
        
        public static Store[] All = new Store[]
                                        {
                                            KomplettNo,
                                            Norek,
                                            KomplettSe,
                                            NorekSe,
                                            KomplettDk,
                                            Inwarehouse,
                                            Mpx,
                                            Itegra
                                        };
    }

    public class Store
    {
       public int StoreId { get; private set; } //Store Id
        public string VTWEG { get; private set; } //Sales Channel 
        public string StoreType { get; private set; } // StoreType - B2B or B2C
        public string ALAND { get; private set; } // Country 
        public string WAERS { get; private set; } // Currency 
        public string SPRAS { get; private set; } // Language

        public Store(int storeId, string vtweg, string storeType, string aland, string waers, string spras)
        {
            WAERS = waers;
            StoreId = storeId;
            VTWEG = vtweg;
            StoreType = storeType;
            ALAND = aland;
            SPRAS = spras;
        }
    }
}