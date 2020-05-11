using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Buffers;

namespace MSLibrary.Collections
{
    public class MemoryList<T>
    {
        private int _perSzie=100;
        private List<Item<T>> _datas = new List<Item<T>>();
      

        public void Add(Memory<T> list)
        {
            Add(Count, list);
        }

        public void Add(int position,Memory<T> list)
        {
            var newItem = new Item<T>()
            {
                Data = list
            };

            var (itemIndex,dataIndex)=getRealPosition(position);
            //如果是新数据块，新增数据块到最后
            if (itemIndex> _datas.Count-1)
            {
                _datas.Add(newItem);
            }
            else if (itemIndex==0 && dataIndex==0)
            {
                //插入到数据块第一个
                _datas.Insert(0,newItem);
            }
            else
            {
                //需要拆分原有数据块
                Item<T> fItem = null;
                if (dataIndex > 0)
                {
                    fItem =new Item<T>() { Data = _datas[itemIndex].Data.Slice(0, dataIndex) };
                }

                var sItem= _datas[itemIndex].Data.Slice(dataIndex, _datas[itemIndex].Data.Length- dataIndex);
                _datas.RemoveAt(itemIndex);

                if (fItem!=null)
                {
                    _datas.Insert(itemIndex, fItem);
                    itemIndex++;
                }
                _datas.Insert(itemIndex, newItem);

                _datas.Insert(itemIndex + 1, new Item<T>()
                {
                    Data = sItem
                });
            }



        }

        public IList<Memory<T>> Slice(int start,int length)
        {
            List<Memory<T>> dataList = new List<Memory<T>>();

            //计算出起始的要移除数据的数据块位置
            var (itemIndex, dataIndex) = getRealPosition(start);
            var (lItemIndex, lDataIndex) = getRealPosition(start + length);

            if (itemIndex > _datas.Count - 1)
            {
                return dataList;
            }


            if (lItemIndex > _datas.Count - 1)
            {
                lItemIndex = _datas.Count - 1;
                lDataIndex = _datas[_datas.Count - 1].Data.Length - 1;
            }

            //计算出要返回的数据块
           

            if (lItemIndex - itemIndex - 1 > 0)
            {
                var datas = (from item in _datas.Skip(itemIndex + 1).Take(lItemIndex - itemIndex - 1)
                             select item.Data).ToList();
                dataList.AddRange(datas);
            }


            Memory<T> fInsert = null;
            //拆分第一块数据块
            if (dataIndex > 0)
            {
                dataList.Add(_datas[itemIndex].Data.Slice(dataIndex, _datas[itemIndex].Data.Length- dataIndex));
            }

            //拆分最后一块数据块
            if (lDataIndex <= _datas[lItemIndex].Data.Length - 1)
            {
                dataList.Add(_datas[lItemIndex].Data.Slice(0, lDataIndex));
            }

            return dataList;

        }

        public int Count
        {
            get
            {

                var count = (from item in _datas
                             select item.Data.Length).Sum();
                return count;
            }
        }

        public void Clear()
        {
            _datas.Clear();
        }

        public void Remove(int start,int length)
        {
            //计算出起始的要移除数据的数据块位置
            var (itemIndex, dataIndex) = getRealPosition(start);
            var (lItemIndex, lDataIndex) = getRealPosition(start + length);

            if (itemIndex > _datas.Count - 1)
            {
                return;
            }


            if (lItemIndex > _datas.Count - 1)
            {
                lItemIndex = _datas.Count - 1;
                lDataIndex = _datas[_datas.Count - 1].Data.Length - 1;
            }

            //计算出要移除的数据块
            IEnumerable<Item<T>> removeItems = null;

            if (lItemIndex - itemIndex-1 > 0)
            {
                removeItems = _datas.Skip(itemIndex + 1).Take(lItemIndex - itemIndex-1);
            }


            Item<T> fInsert = null;
            //拆分第一块数据块
            if (dataIndex>0)
            {
                fInsert = new Item<T>() { Data = _datas[itemIndex].Data.Slice(0, dataIndex) };
            }

            //拆分最后一块数据块
            Item<T> lInsert = null;
            if (lDataIndex <_datas[lItemIndex].Data.Length-1)
            {
                lInsert = new Item<T>() { Data = _datas[lItemIndex].Data.Slice(lDataIndex, _datas[lItemIndex].Data.Length - lDataIndex) };
            }

            Item<T> fDelete = _datas[itemIndex];
            Item<T> lDelete=null;
            if (itemIndex != lItemIndex)
            {
                lDelete = _datas[lItemIndex];
            }

            _datas.Remove(fDelete);
            if (lDelete!=null)
            {
                _datas.Remove(lDelete);
            }

            if (removeItems != null)
            {
                foreach (var item in removeItems)
                {
                    _datas.Remove(item);
                }
            }

            if (fInsert != null)
            {
                _datas.Insert(itemIndex, fInsert);
                itemIndex++;
            }

            if (lInsert!=null)
            {
                _datas.Insert(itemIndex, lInsert);
            }


        }

        /// <summary>
        /// 计算逻辑位置对应的真实位置
        /// 获取真实位置所在的数据块的index、该数据块中数据的位置
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private (int,int) getRealPosition(int position)
        {
            int itemIndex=0;
            int startIndex=0;
            int realIndex = 0;
            bool exist = false;
            for(var index=0;index<=_datas.Count-1;index++)
            {
                itemIndex = index;
                if (position>= realIndex && position<= realIndex+ _datas[index].Data.Length-1)
                {
                    startIndex=position - realIndex;
                    exist = true;
                    break;
                }

                realIndex = realIndex + _datas[index].Data.Length;
            }

            if (!exist)
            {
                startIndex = 0;
                itemIndex += 1;
            }

            return (itemIndex, startIndex);
        }



        private class Item<T>
        {
            public Memory<T> Data
            {
                get;set;
            }

        }
    }
}
