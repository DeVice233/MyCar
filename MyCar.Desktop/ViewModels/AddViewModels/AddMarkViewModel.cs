﻿using ModelsApi;
using MyCar.Desktop.Core;
using MyCar.Desktop.Core.UI;
using MyCar.Desktop.ViewModels.Dialogs;
using MyCar.Desktop.Windows.AddWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyCar.Desktop.ViewModels.AddViewModels
{
    public class AddMarkViewModel : BaseViewModel
    {
        private string searchText = "";
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Task.Run(Search);
            }
        }
        private ObservableCollection<ModelApi> searchResult;
        public MarkCarApi AddMark { get; set; }

        public List<MarkCarApi> Marks { get; set; }
        public ObservableCollection<ModelApi> AllModels { get; set; }
        public ObservableCollection<ModelApi> ThisMarkModels { get; set; } = new ObservableCollection<ModelApi>();

        public ModelApi SelectedModel { get; set; }
        public ModelApi SelectedMarkModel { get; set; }

        public CustomCommand Cancel { get; set; }
        public CustomCommand Save { get; set; }
        public CustomCommand AddModel { get; set; }
        public CustomCommand RemoveModel { get; set; }
        public CustomCommand EditModel { get; set; }
        public CustomCommand AddNewModel { get; set; }
        public AddMarkViewModel(MarkCarApi addmark)
        {
            Task.Run(GetList);

            if (addmark == null)
            {
                AddMark = new MarkCarApi();
            }
            else
            {
                AddMark = new MarkCarApi
                {
                    ID = addmark.ID,
                    MarkName = addmark.MarkName,
                    Models = addmark.Models,
                };
                ThisMarkModels = new ObservableCollection<ModelApi>(AddMark.Models);
            }

            AddModel = new CustomCommand(() =>
            {
                if (SelectedModel == null) return;
                if (CheckContains(ThisMarkModels.ToList(), SelectedModel))
                {
                    SendMessage($"Модель {SelectedModel.ModelName} уже содержится в марке");
                }
                MarkCarApi m = Marks.FirstOrDefault(x => x.ID == SelectedModel.MarkId);

                MessageBoxDialogViewModel result = new MessageBoxDialogViewModel
                { Title = "Подтверждение", Message = $"{SelectedModel.ModelName} уже содержится в {m.MarkName}, заменить?" };
                UIManager.ShowMessageYesNo(result);
                if (result.Result)
                {
                    ThisMarkModels.Add(SelectedModel);
                }
            });

            EditModel = new CustomCommand(async () =>
            {
                if (SelectedModel == null || SelectedModel.ID == 0) return;
                AddModelWindow addModel = new AddModelWindow(SelectedModel);
                addModel.ShowDialog();
                await Task.Run(GetList);
                UpdateListBox();
            });

            RemoveModel = new CustomCommand(() =>
            {
                if (SelectedMarkModel == null) return;
                ThisMarkModels.Remove(SelectedMarkModel);
            });

            Save = new CustomCommand(async () =>
            {
                foreach (var mark in Marks)
                {
                    if (mark.MarkName == AddMark.MarkName)
                    {
                        SendMessage("Такая марка уже существует!");
                    }
                }

                try
                {
                    if (AddMark.ID == 0)
                        await Add(AddMark);
                    else
                        await Edit(AddMark);
                }
                catch (Exception e)
                {
                    SendMessage($"{e.Message.ToString()}");
                }
                UIManager.CloseWindow(this);
            });
            Cancel = new CustomCommand(() =>
            {
                UIManager.CloseWindow(this);
            });
            

        }

        private async Task Add(MarkCarApi mark)
        {
            int id = await Api.PostAsync<MarkCarApi>(mark, "MarkCar");
            foreach (ModelApi model in ThisMarkModels)
            {
                model.MarkId = id;
                await Api.PutAsync<ModelApi>(model, "Model");
            }
        }
        private async Task Edit(MarkCarApi mark)
        {
            foreach (ModelApi model in ThisMarkModels)
            {
                model.MarkId = AddMark.ID;
                await Api.PutAsync<ModelApi>(model, "Model");
            }
            var id = await Api.PutAsync<MarkCarApi>(mark, "MarkCar");
        }
        public async Task Search()
        {
            var search = SearchText.ToLower();
            if (search == "")
                searchResult = await Api.GetListAsync<ObservableCollection<ModelApi>>("Model");
            else
                searchResult = await Api.SearchAsync<ObservableCollection<ModelApi>>("Наименование", search, "Model");
            UpdateList();
        }
        private void UpdateList()
        {
            AllModels = searchResult;
            SignalChanged(nameof(AllModels));
        }

        private bool CheckContains(List<ModelApi> list, ModelApi model)
        {
            bool result = false;
            foreach (ModelApi item in list)
            {
                if (item.ID == model.ID)
                {
                    result = true;
                }
            }
            return result;
        }

        private void UpdateListBox()
        {
            ThisMarkModels.Clear();
            foreach (var model in AllModels)
            {
                if (model.MarkId == AddMark.ID)
                {
                    ThisMarkModels.Add(model);
                }
            }
            SignalChanged(nameof(ThisMarkModels));
        }
        private async Task GetList()
        {
            var list = await Api.GetListAsync<List<ModelApi>>("Model");
            AllModels = new ObservableCollection<ModelApi>(list);
            Marks = await Api.GetListAsync<List<MarkCarApi>>("MarkCar");
            SignalChanged(nameof(AllModels));
        }

        private void SendMessage(string message)
        {
            UIManager.ShowMessage(new Dialogs.MessageBoxDialogViewModel
            {
                Message = message,
                OkText = "ОК",
                Title = "Ошибка!"
            });
            UIManager.CloseWindow(this);
            return;
        }
    }
}