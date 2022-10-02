using System;

namespace app
{
    class Program
    {
        static List<NeatMain> Brains = new List<NeatMain>();
        
        static void Main(string[] args)
        {
            DataPoint[] trainData = xor(100);
            
            /*for (int i = 0; i < 784; i++)
            {
                Console.WriteLine(trainData[11].inputs[i]);
            } */
            
            
            
            

            for (int i = 0; i < 10; i++)
                        {
                           evokution(1000,10,10000,batchMaker(trainData,40),8000);
                            double cost = 0;
                            double mycost;
                            double minima = 100;
                            int indexOfMinima = 0;
                            int v = 0;
                            foreach (NeatMain Brain in Brains)
                            {
                                
                                mycost = Brain.valueAll(batchMaker(trainData,40));
                                cost += mycost;
                                if(mycost<minima){
                                    minima = mycost;
                                    indexOfMinima = v;
                                }

                                v++;
                            }
                            Brains[indexOfMinima].isBest=true;
                            Console.WriteLine("current amount of brains " + Brains.Count);
                            Console.WriteLine("average cost" + cost/Brains.Count);
                            Console.WriteLine("minima "+ minima);
                            Console.WriteLine("Connections of Best"+ Brains[indexOfMinima].connections.Count);
                            Console.WriteLine("Nodes of Best"+ Brains[indexOfMinima].HiddenNodes.Count);
                            

                        }
        
        int z = 0;
        foreach (NeatMain Brain in Brains){
            if(Brains[z].isBest||Brains[z].lastPerformance<=0.3){
                Console.WriteLine("This is Brain "+z+" with performance "+ Brains[z].lastPerformance);
                for (int i = 0; i < 10; i++)
                    {
                                Console.WriteLine("input 1 "+trainData[i].inputs[0]);
                                Console.WriteLine("input 2 "+trainData[i].inputs[1]);
                                Console.WriteLine("neural output 1 "+Brain.calculate(trainData[i].inputs)[0]);
                                Console.WriteLine("neural output 2 "+Brain.calculate(trainData[i].inputs)[1]);
                                Console.WriteLine("lable 1 "+trainData[i].expectedOutputs[0]);
                                Console.WriteLine("lable 2 "+trainData[i].expectedOutputs[1]);
                    }
            }
            z++;
        }
            

            

            //testNet.valueAll(trainData);
           //firstNetwork.classifyAll(creatDataPicturesTest());
           //Console.WriteLine(firstNetwork.cost(trainData));
          
            //firstNetwork.Learn(creatData(),0.0001);
            //firstNetwork.Learn(creatData(),0.0001);
            
           /* for (int i = 0; i < 100; i++)
            {
                DataPoint[] testData = batchMaker(trainData,100);
                firstNetwork.Learn(testData,2);
                if(i%500 == 0){
                    Console.WriteLine(firstNetwork.cost(testData));
                }
                
            }

            foreach (DataPoint data in realTestData)
            {
                Console.WriteLine(firstNetwork.Classify(data.inputs));
                Console.WriteLine("real:" + data.label);

            }
            Console.WriteLine(firstNetwork.cost(realTestData));

*/
            

            
            

        

        }

        

        public static void removeBadBrains(DataPoint[] dataPoints,int whenToKill,int topWhat,int maxBrainCount){
            
            
            
            double[] valueAt = new double[Brains.Count];
            
            for (int i = 0; i < Brains.Count; i++)
            {
                if (Brains[i].isMutated)
                {
                   valueAt[i]=Brains[i].valueAll(dataPoints);
                    //valueAt[i]+= 0.001*Brains[i].HiddenNodes.Count; 
                }
                
            }
            /*for (int i = 0; i < Brains.Count; i++)
            {
                valueAt[i]+=Brains[i].valueAll(dataPoints);
                valueAt[i]+= 0.01*Brains[i].HiddenNodes.Count;
            }*/



            Array.Sort(valueAt);
            //Console.WriteLine("absolute best "+ valueAt[0]);
            int BrainCount = Brains.Count;
            
            for (int i = 0; i < BrainCount; i++)
            {
                
                
                //valueAt[800]>Brains[i].lastPerformance&&
               // Console.WriteLine(Brains.Count);
                if(Brains.Count>=3000&&valueAt[1000]>Brains[i].lastPerformance&&Brains.Count<=maxBrainCount&&Brains[i].lastPerformance>0)
                {
                    //Console.WriteLine("Brains.Count");
                    Brains.Add(Brains[i].CloneMe());
                   if(Brains.Count>=3000&&valueAt[500]>Brains[i].lastPerformance&&Brains.Count<=maxBrainCount&&Brains[i].lastPerformance>0)
                    {
                    //Console.WriteLine("Brains.Count");
                    Brains.Add(Brains[i].CloneMe());
                        if(Brains.Count>=3000&&valueAt[1]>Brains[i].lastPerformance&&Brains.Count<=maxBrainCount&&Brains[i].lastPerformance>0)
                        {
                        //Console.WriteLine("Brains.Count");
                        Brains.Add(Brains[i].CloneMe());
                        Brains[i].isBest = true;
                        //Console.WriteLine("New Best is "+i+ " with Score "+Brains[i].lastPerformance);
                        
                        }
                    
                    }  
                    
                }  
                if (valueAt[whenToKill]<Brains[i].lastPerformance&&Brains.Count>=300)
                {
                    
                    Brains.RemoveAt(i);
                    
                    i--;
                    BrainCount--;
                }    
            }
            

            
        
        
            //Brains.AddRange(TopBrains);*/
        }

        public static void evokution(int epochAmounts,int mutationRate,int brainCount,DataPoint[] dataPoints,int whenToKill){
            for (int i = 0; i < epochAmounts; i++)
            {
                
                keepAt(brainCount);
                
                mutate(mutationRate);
                
                removeBadBrains(dataPoints,whenToKill,100,11000);

                mutate(mutationRate);
                //repoduceGoodStuff(dataPoints,100,1000);                
            }
        }

        public static void keepAt(int amount){
            if (amount>Brains.Count)
            {
                Brains.Add(new NeatMain(2,2));
                keepAt(amount);
                
            }
            else
            {
                return;
            }
        }
        public static void mutate(int rate){
            foreach (NeatMain Brain in Brains)
            {
                Brain.mutateNet(rate);
            }
        }



        public static DataPoint[] creatData(int amount){
            System.Random rng = new System.Random();
            DataPoint[] completeData = new DataPoint[amount];
            for(int i = 0;i<amount;i++){
                double [] inputData = new double[2];
                int randomNumberInt = rng.Next(10);
                double randomNumber = (double)randomNumberInt;
                
                
                

                if(rng.Next(2)==1){
                    
                    inputData[0] = randomNumber;
                    inputData[1] = randomNumber*randomNumber;
                    completeData[i] = new DataPoint(inputData,1,2);
                }
                else{
                    int randomNumberInt2 = rng.Next(10);
                    if (randomNumberInt2 == randomNumberInt)
                    {
                        randomNumberInt2 = rng.Next(10);
                    }
                    double randomNumber2 = (double)randomNumberInt2;
                    inputData[0] = randomNumber;
                    inputData[1] = randomNumber*randomNumber2;
                    completeData[i] = new DataPoint(inputData,0,2);
                }
                
            }
            
            
            return completeData;
        }

        public static DataPoint[] creatDataPictures(){
            
            DataPoint[] completeDataTrainng = new DataPoint[60000];
            int i = 0;
            foreach (var image in MnistReader.ReadTrainingData()){
                double [] inputDataImage = new double[image.Data.Length];
                inputDataImage = Extensions.conv2dTo1d(image.Data,784,28,28);                
                completeDataTrainng[i] = new DataPoint(inputDataImage,image.Label,10);
                i++;
            }
            
            return completeDataTrainng;
        }

        public static DataPoint[] creatDataPicturesTest(){
            //System.Random rng = new System.Random();
            DataPoint[] completeDataTrainng = new DataPoint[10000];
            int i = 0;
            foreach (var image in MnistReader.ReadTestData()){
                double [] inputDataImage = new double[image.Data.Length];
                inputDataImage = Extensions.conv2dTo1d(image.Data,784,28,28);
                completeDataTrainng[i] = new DataPoint(inputDataImage,image.Label,10);
                i++;
            }
            return completeDataTrainng;
        }

        public static DataPoint[] xor(int amount){
           System.Random rng = new System.Random();
            DataPoint[] completeData = new DataPoint[amount];
            for(int i = 0;i<amount;i++){
                double [] inputData = new double[2];
                int randomNumberInt = rng.Next(4);
                //double randomNumber = (double)randomNumberInt;
                
                
                

                if(randomNumberInt==0){
                    
                    inputData[0] = 0;
                    inputData[1] = 0;
                    completeData[i] = new DataPoint(inputData,0,2);
                }
                if(randomNumberInt==1){
                    
                    inputData[0] = 0;
                    inputData[1] = 1;
                    completeData[i] = new DataPoint(inputData,1,2);
                }
                if(randomNumberInt==2){
                    
                    inputData[0] = 1;
                    inputData[1] = 0;
                    completeData[i] = new DataPoint(inputData,1,2);
                }
                else{
                    inputData[0] = 1;
                    inputData[1] = 1;
                    completeData[i] = new DataPoint(inputData,0,2);
                }
                
            }
            
            
            return completeData;
        } 
        


        public static DataPoint[] batchMaker(DataPoint[] fullData, int batchSize){
            DataPoint[] batchData = new DataPoint[batchSize];
            System.Random rng = new System.Random();
            for (int i = 0; i < batchSize; i++)
            {
                int rndIndex = rng.Next(fullData.Length);
                batchData[i] = fullData[rndIndex];
            }
            return batchData;
        }

    }
}