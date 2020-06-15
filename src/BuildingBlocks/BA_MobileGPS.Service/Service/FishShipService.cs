using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BA_MobileGPS.Service
{
    public class FishShipService : IFishShipService
    {
        private readonly IRealmBaseService<FishTripRealm, FishTrip> fishTripRepository;
        private readonly IRealmBaseService<FishTripQuantityRealm, FishTripQuantity> fishTripQuantityRepository;
        private readonly IRequestProvider RequestProvider;

        public FishShipService(IRequestProvider requestProvider, IRealmBaseService<FishTripRealm, FishTrip> fishTripRepository,
            IRealmBaseService<FishTripQuantityRealm, FishTripQuantity> fishTripQuantityRepository)
        {
            RequestProvider = requestProvider;
            this.fishTripRepository = fishTripRepository;
            this.fishTripQuantityRepository = fishTripQuantityRepository;
        }

        public List<FishTrip> GetListFishTrip()
        {
            return fishTripRepository.All().ToList();
        }

        public List<FishTrip> GetListFishTrip(Expression<Func<FishTripRealm, bool>> predicate)
        {
            return fishTripRepository.Find(predicate).ToList();
        }

        public List<FishTripQuantity> GetFishTripDetail(long fishTripID)
        {
            return fishTripQuantityRepository.Find(f => f.Parent == fishTripID).ToList();
        }

        public bool SaveFishTrip(FishTrip fishTrip, IList<FishTripQuantity> fishTripDetail)
        {
            try
            {
                fishTrip.SysLastChangeDate = DateTimeOffset.Now;
                var result = fishTripRepository.Add(fishTrip);
                foreach (var item in fishTripDetail)
                {
                    item.Parent = result.Id;
                    item.SysLastChangeDate = result.SysLastChangeDate;
                    fishTripQuantityRepository.Add(item);
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);

                return false;
            }

            return true;
        }

        public bool UpdateFishTrip(FishTrip fishTrip, IList<FishTripQuantity> fishTripDetail)
        {
            try
            {
                fishTrip.SysLastChangeDate = DateTimeOffset.Now;
                var result = fishTripRepository.Update(fishTrip);
                foreach (var item in fishTripDetail)
                {
                    item.Parent = result.Id;
                    item.SysLastChangeDate = result.SysLastChangeDate;
                    if (fishTripQuantityRepository.Any(f => f.Id == item.Id))
                    {
                        fishTripQuantityRepository.Update(item);
                    }
                    else
                    {
                        fishTripQuantityRepository.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);

                return false;
            }

            return true;
        }

        public bool DeleteFishTrip(FishTrip fishTrip)
        {
            try
            {
                fishTrip.IsDeleted = true;
                fishTrip.SysLastChangeDate = DateTimeOffset.Now;
                fishTripRepository.Update(fishTrip);
                foreach (var item in fishTripQuantityRepository.Find(q => q.Parent == fishTrip.Id))
                {
                    item.IsDeleted = true;
                    item.SysLastChangeDate = fishTrip.SysLastChangeDate;
                    fishTripQuantityRepository.Update(item);
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);

                return false;
            }

            return true;
        }

        public Task SyncFishTrip(FishTrip fishTrip, IList<FishTripQuantity> fishTripDetail)
        {
            return Task.Run(async () =>
            {
                return await SendFishStrip(new SendFishTripRequest
                {
                    FishTrip = fishTrip,
                    ListFish = fishTripDetail
                });
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    if (task.Result != 0)
                    {
                        fishTrip.PK_FishTrip = task.Result;
                        fishTrip.SysLastChangeDate = fishTrip.LastSynchronizationDate = DateTimeOffset.Now;
                        fishTripRepository.Update(fishTrip);
                        foreach (var item in fishTripDetail)
                        {
                            item.FK_FishTripID = fishTrip.PK_FishTrip;
                            item.SysLastChangeDate = item.LastSynchronizationDate = fishTrip.SysLastChangeDate;
                            fishTripQuantityRepository.Update(item);
                        }
                    }
                }
            }));
        }

        public async Task<List<Fish>> GetListFish()
        {
            List<Fish> result = default;
            try
            {
                result = new List<Fish>
                {
                    new Fish
                    {
                        PK_FishID = 1,
                        Code = "BCAT",
                        Name = "Cá bơn cát"
                    },
                    new Fish
                    {
                        PK_FishID = 2,
                        Code = "BDD",
                        Name = "Cá bò đuôi dài"
                    },
                    new Fish
                    {
                        PK_FishID = 3,
                        Code = "BDIEU",
                        Name = "Cá bạch điều"
                    },
                    new Fish
                    {
                        PK_FishID = 4,
                        Code = "BGL",
                        Name = "Cá bò một gai lưng"
                    },
                    new Fish
                    {
                        PK_FishID = 5,
                        Code = "BMA",
                        Name = "Cá bạc má"
                    },
                    new Fish
                    {
                        PK_FishID = 6,
                        Code = "BMAO",
                        Name = "Cá bơn mào"
                    },
                    new Fish
                    {
                        PK_FishID = 7,
                        Code = "BNGO",
                        Name = "Cá bơn ngộ"
                    },
                    new Fish
                    {
                        PK_FishID = 8,
                        Code = "BTHU",
                        Name = "Cá ba thú"
                    },
                    new Fish
                    {
                        PK_FishID = 9,
                        Code = "BVRT",
                        Name = "Cá bơn vằn răng to"
                    },
                    new Fish
                    {
                        PK_FishID = 10,
                        Code = "CAM",
                        Name = "Cá cam"
                    },
                    new Fish
                    {
                        PK_FishID = 11,
                        Code = "CAMS",
                        Name = "Cá cam sọc"
                    },
                    new Fish
                    {
                        PK_FishID = 12,
                        Code = "CAMT",
                        Name = "Cá cam thoi"
                    },
                    new Fish
                    {
                        PK_FishID = 13,
                        Code = "CAMV",
                        Name = "Cá cam vân"
                    },
                    new Fish
                    {
                        PK_FishID = 14,
                        Code = "CHAI",
                        Name = "Cá chai"
                    },
                    new Fish
                    {
                        PK_FishID = 15,
                        Code = "CHEM",
                        Name = "Cá chẽm"
                    },
                    new Fish
                    {
                        PK_FishID = 16,
                        Code = "CHIMD",
                        Name = "Cá chim đen"
                    },
                    new Fish
                    {
                        PK_FishID = 17,
                        Code = "CHIMG",
                        Name = "Cá chim gai"
                    },
                    new Fish
                    {
                        PK_FishID = 18,
                        Code = "CHIMT",
                        Name = "Cá chim trắng"
                    },
                    new Fish
                    {
                        PK_FishID = 19,
                        Code = "CHIV",
                        Name = "Cá chỉ vàng"
                    },
                    new Fish
                    {
                        PK_FishID = 20,
                        Code = "COAD",
                        Name = "Cá cờ ấn độ"
                    },
                    new Fish
                    {
                        PK_FishID = 21,
                        Code = "COLX",
                        Name = "Cá cờ xanh"
                    },
                    new Fish
                    {
                        PK_FishID = 22,
                        Code = "COMAD",
                        Name = "Cá cơm ấn độ"
                    },
                    new Fish
                    {
                        PK_FishID = 23,
                        Code = "COMS",
                        Name = "Cá cơm săng"
                    },
                    new Fish
                    {
                        PK_FishID = 24,
                        Code = "COMT",
                        Name = "Cá cơm thường"
                    },
                    new Fish
                    {
                        Code = "COMTH",
                        PK_FishID = 25,
                        Name = "Cá cơm trung hoa"
                    },
                    new Fish
                    {
                        PK_FishID = 26,
                        Code = "CVT",
                        Name = "Cá căng vảy to"
                    },
                    new Fish
                    {
                        PK_FishID = 27,
                        Code = "CVV",
                        Name = "Cá chuồn vây vàng"
                    },
                    new Fish
                    {
                        PK_FishID = 28,
                        Code = "DAM",
                        Name = "Cá dầm"
                    },
                    new Fish
                    {
                        PK_FishID = 29,
                        Code = "DBDV",
                        Name = "Cá đuối bống đuôi vằn"
                    },
                    new Fish
                    {
                        PK_FishID = 30,
                        Code = "DDL",
                        Name = "Cá đỏ dạ lớn"
                    }
                };
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<int> SendFishStrip(SendFishTripRequest request)
        {
            try
            {
                return await RequestProvider.PostAsync<SendFishTripRequest, int>(ApiUri.SEND_FISH_TRIP, request);
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return 0;
        }
    }
}