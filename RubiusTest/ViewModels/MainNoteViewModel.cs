using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Ascon.Pilot.SDK;
using RubiusTest.Helpers;
using RubiusTest.Models;
using RubiusTest.Views;
using Telerik.Windows.Controls;

namespace RubiusTest.ViewModels

{
    public class MainNoteViewModel : NotifyPropertyChangedBase
    {
        private readonly IObjectsRepository _repository;
        private readonly IPilotDialogService _pilotDialogService;
        private readonly ITabServiceProvider _tabServiceProvider;
        private readonly IObjectModifier _modifier;

        private IType note_Type;
        private IType note;

        

        public MainNoteViewModel(IObjectsRepository repository, IPilotDialogService pilotDialogService, ITabServiceProvider tabServiceProvider, IObjectModifier modifier)
        {
            _repository = repository;
            _pilotDialogService = pilotDialogService;
            _tabServiceProvider = tabServiceProvider;
            _modifier = modifier;

            OpenNoteCommand = new RelayCommand(OpenNote, obj => SelectedNote != null);
            CreateNoteCommand = new RelayCommand(CreateNote, obj => NotesList != null);
            RemoveNoteCommand = new RelayCommand(DeleteNote, obj=> CanExecuteRemoveEdit());
            EditNoteCommand = new RelayCommand(EditNote, obj => CanExecuteRemoveEdit());
            CancelCommand = new RelayCommand(CancelEdit);
            OkCommand = new RelayCommand(FinishEdit);
            
            Init();
        }

        private bool CanExecuteRemoveEdit()
        {
            return SelectedNote != null && SelectedNote.Author.Id == CurrentPerson.Id;
        }
        #region ICommands
       
        public ICommand RemoveNoteCommand { get; }
        public ICommand CreateNoteCommand { get; }
        public ICommand EditNoteCommand { get; }
        public ICommand OpenNoteCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        #endregion


        #region variables

        /// <summary>
        /// Окно редактирования и добавления заметки
        /// </summary>
        private NoteEditorView _noteEditorView;
        
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public IPerson CurrentPerson => _repository.GetCurrentPerson();

        private ObservableCollection<NoteModel> _notesList;
        /// <summary>
        /// Список всех заметок
        /// </summary>
        public ObservableCollection<NoteModel> NotesList
        {
            get { return _notesList; }
            set { _notesList = value; RaisePropertyChanged(nameof(NotesList)); }
        }

        private NoteModel _selectedNote;
        /// <summary>
        /// Список всех заметок
        /// </summary>
        public NoteModel SelectedNote
        {
            get { return _selectedNote; }
            set { _selectedNote = value; RaisePropertyChanged(nameof(SelectedNote)); }
        }

        private bool _isNewNote;
        /// <summary>
        /// Признак добавления новой записи
        /// </summary>
        public bool IsNewNote
        {
            get { return _isNewNote; }
            set
            {
                _isNewNote = value; RaisePropertyChanged(nameof(IsNewNote));
            }
        }

        #endregion
        
        #region Commands
        public void Init(object arg = null)
        {
            //todo DataAccess
            
            NotesList = new ObservableCollection<NoteModel>()
            {
                new NoteModel()
                {
                    Id = new Guid(),
                    Topic = "Заметка 1",
                    Content = "Эту заметку невозможно удалить, так как создатель - не текущий пользователь.",
                    CreationDate = DateTime.Now,
                    LastEditDate = DateTime.Now,
                    Author = _repository.GetPeople().FirstOrDefault(x=>x.Id != CurrentPerson.Id)
                },
                new NoteModel()
                {
                    Id = new Guid(),
                    Topic = "Заметка 2",
                    Content = "Эта \r\nзаметка \r\nсодержит \r\nмногострочный контент, просмотреть его можно открыв заметку.\r\n" +
                              "Ощущение мира решительно контролирует непредвиденный гравитационный парадокс. " +
                              "Отсюда естественно следует, что автоматизация дискредитирует предмет деятельности. " +
                              "Структурализм абстрактен. Отсюда естественно следует, что автоматизация дискредитирует предмет деятельности. " +
                              "Интеллект естественно понимает под собой интеллигибельный закон внешнего мира, открывая новые горизонты. " +
                              "Структурализм абстрактен. Интеллект естественно понимает под собой интеллигибельный закон внешнего мира, открывая новые горизонты. " +
                              "Дедуктивный метод решительно представляет собой бабувизм.",
                    CreationDate = DateTime.Now,
                    LastEditDate = DateTime.Now,
                    Author = CurrentPerson
                },
                new NoteModel()
                {
                    Id = new Guid(),
                    Topic = "Заметка 3",
                    Content = "Отредактировав заметку - изменится дата последнего редактирования.",
                    CreationDate = DateTime.Now.AddDays(-1),
                    LastEditDate = DateTime.Now.AddDays(-1),
                    Author = CurrentPerson
                },
            };

            for (var i = 4; i < 15; i++)
            {
                NotesList.Add(new NoteModel()
                {
                    Id = new Guid(),
                    Topic = $"Заметка {i}",
                    Content = "Fortune day out married parties. If in so bred at dare rose lose good. Painful so he an comfort is manners. Draw fond rank form nor the day eat. Celebrated delightful an especially increasing instrument am. ",
                    CreationDate = DateTime.Now,
                    LastEditDate = DateTime.Now,
                    Author = CurrentPerson
                });
            }
        }

        public void OpenNote(object arg = null)
        {
            var viewer = new NoteViewerView()
            {
                ResizeMode = ResizeMode.NoResize,
                DataContext = new NoteViewerViewModel(SelectedNote)
            };
            viewer.ShowDialog();
        }

        /// <summary>
        /// Создание новой заметки
        /// </summary>
        /// <param name="arg"></param>
        public void CreateNote(object arg = null)
        {
            IsNewNote = true;
            var newNote = new NoteModel()
            {
                Id = new Guid(),
                CreationDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                Author = _repository.GetCurrentPerson()
            };
            _noteEditorView = new NoteEditorView
            {
                ResizeMode = ResizeMode.NoResize,
                DataContext = new NoteEditorViewModel(newNote, OkCommand, CancelCommand)
            };

            _noteEditorView.ShowDialog();
        }

        public void EditNote(object arg = null)
        {
            IsNewNote = false;
            SelectedNote.BeginEdit();

            _noteEditorView = new NoteEditorView
            {
                ResizeMode = ResizeMode.NoResize,
                CanClose = false,
                DataContext = new NoteEditorViewModel(SelectedNote, OkCommand, CancelCommand)
            };
            _noteEditorView.ShowDialog();
        }

        /// <summary>
        /// Удаление заметки
        /// </summary>
        /// <param name="arg"></param>
        public void DeleteNote(object arg = null)
        {
            var confirmText = $"Удалить заметку {SelectedNote.Topic}?";
            RadWindow.Confirm(confirmText, OnConfirmClosed);
        }



        /// <summary>
        /// Завершение создания/редактирования заметки
        /// </summary>
        /// <param name="arg"></param>
        private void FinishEdit(object arg = null)
        {
            if (IsNewNote)
            {
                if (arg is NoteModel editedNote)
                {
                    NotesList.Add(editedNote);
                }
            }
            else
            {
                SelectedNote.LastEditDate = DateTime.Now;
            }
            
           
            _noteEditorView.Close();
        }

        /// <summary>
        /// Отмена создания/редактирования заметки
        /// </summary>
        /// <param name="arg"></param>
        public void CancelEdit(object arg = null)
        {
            _noteEditorView.Close();
            if (!IsNewNote)
            {
                (arg as NoteModel)?.CancelEdit();
            }
            
            IsNewNote = false;
        }
        
        #endregion

        /// <summary>
        /// Результат диалогового окна удаления заметки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConfirmClosed(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                NotesList.Remove(SelectedNote);
            }
        }



    }
}
