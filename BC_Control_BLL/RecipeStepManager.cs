using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;
using BC_Control_Models.RecipeModel.RecipeBase;

namespace BC_Control_BLL
{
    public class RecipeStepManager<T> where T : class, IRecipeStep, new()
    {
        private readonly IList<T> _steps;

        public RecipeStepManager(IList<T> initialSteps)
        {
            _steps = initialSteps;
            SortSteps();
        }

        // 获取排序后的步骤列表
        public List<T> GetSortedSteps()
        {
            return _steps.OrderBy(s => s.StepType)
                        .ThenBy(s => s.StepNo)
                        .ToList();
        }

        // 插入步骤
        public bool InsertStep(T newStep)
        {
            try
            {
                // 检查是否已存在相同类型和序号的步骤
                var existingStep = _steps.FirstOrDefault(s =>
                    s.StepType == newStep.StepType && s.StepNo == newStep.StepNo);

                if (existingStep != null)
                {
                    // 如果已存在，调整后续步骤的序号
                    AdjustStepNumbers(newStep.StepType, newStep.StepNo, 1);
                }

                _steps.Add(newStep);
                SortSteps();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 插入步骤到指定位置
        public bool InsertStepAt(ProcessStepEnum stepType, int position, T newStep)
        {
            try
            {
                newStep.StepType = stepType;
                newStep.StepNo = position;

                // 调整该类型中从position开始的所有步骤的序号
                AdjustStepNumbers(stepType, position, 1);

                _steps.Add(newStep);
                SortSteps();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 删除步骤
        public bool DeleteStep(ProcessStepEnum stepType, int stepNo)
        {
            try
            {
                var stepToRemove = _steps.FirstOrDefault(s =>
                    s.StepType == stepType && s.StepNo == stepNo);

                if (stepToRemove == null)
                    return false;

                _steps.Remove(stepToRemove);

                // 调整该类型中后续步骤的序号
                AdjustStepNumbers(stepType, stepNo, -1);

                SortSteps();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 删除指定类型的所有步骤
        public bool DeleteStepsByType(ProcessStepEnum stepType)
        {
            try
            {
                //_steps.RemoveAll(s => s.StepType == stepType);
                //// 重新整理该类型步骤的序号（如果需要的话）
                //ReorganizeStepNumbers(stepType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 创建新步骤
        public T CreateNewStep(ProcessStepEnum stepType)
        {
            // 获取该类型中最大的步骤序号
            int maxStepNo = _steps.Where(s => s.StepType == stepType)
                                 .Select(s => s.StepNo)
                                 .DefaultIfEmpty(0)
                                 .Max();

            // 创建一个简单的实现类
            var newStep = new T
            {
                StepType = stepType,
                StepNo = maxStepNo + 1,               
            };

            return newStep;
        }

        // 创建并添加新步骤
        public bool CreateAndAddStep(ProcessStepEnum stepType)
        {
            var newStep = CreateNewStep(stepType);
            return InsertStep(newStep);
        }

        // 移动步骤
        public bool MoveStep(ProcessStepEnum fromStepType, int fromStepNo,
                            ProcessStepEnum toStepType, int toStepNo)
        {
            try
            {
                var stepToMove = _steps.FirstOrDefault(s =>
                    s.StepType == fromStepType && s.StepNo == fromStepNo);

                if (stepToMove == null)
                    return false;

                // 删除原位置的步骤
                _steps.Remove(stepToMove);

                // 调整原类型中后续步骤的序号
                AdjustStepNumbers(fromStepType, fromStepNo, -1);

                // 设置新位置
                stepToMove.StepType = toStepType;
                stepToMove.StepNo = toStepNo;

                // 调整目标类型中从toStepNo开始的所有步骤的序号
                AdjustStepNumbers(toStepType, toStepNo, 1);

                // 添加步骤到新位置
                _steps.Add(stepToMove);
                SortSteps();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 获取指定类型的步骤
        public List<T> GetStepsByType(ProcessStepEnum stepType)
        {
            return _steps.Where(s => s.StepType == stepType)
                        .OrderBy(s => s.StepNo)
                        .ToList();
        }

        // 获取步骤总数
        public int GetTotalStepCount()
        {
            return _steps.Count;
        }

        // 获取指定类型的步骤数量
        public int GetStepCountByType(ProcessStepEnum stepType)
        {
            return _steps.Count(s => s.StepType == stepType);
        }

        // 私有方法：调整步骤序号
        private void AdjustStepNumbers(ProcessStepEnum stepType, int startFrom, int adjustment)
        {
            var stepsToAdjust = _steps.Where(s =>
                s.StepType == stepType && s.StepNo >= startFrom);

            foreach (var step in stepsToAdjust)
            {
                step.StepNo += adjustment;
            }
        }

        // 私有方法：重新整理指定类型步骤的序号
        private void ReorganizeStepNumbers(ProcessStepEnum stepType)
        {
            var stepsOfType = _steps.Where(s => s.StepType == stepType)
                                   .OrderBy(s => s.StepNo)
                                   .ToList();

            for (int i = 0; i < stepsOfType.Count; i++)
            {
                stepsOfType[i].StepNo = i + 1;
            }
        }
        private void SortSteps()
        {
            var sortedSteps = _steps.OrderBy(s => s.StepType)
                       .ThenBy(s => s.StepNo)
                       .ToList();

            // 清空原列表并重新添加（保持引用不变）
            _steps.Clear();
            foreach (var item in sortedSteps)
            {
                _steps.Add(item);
            }
        }
    }
}
