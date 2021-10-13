import { NgModule } from '@angular/core';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { CalendarModule } from 'primeng/calendar';
import { SliderModule } from 'primeng/slider';
import { MultiSelectModule } from 'primeng/multiselect';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { ProgressBarModule } from 'primeng/progressbar';
import { InputTextModule } from 'primeng/inputtext';
import { FileUploadModule } from 'primeng/fileupload';
import { ToolbarModule } from 'primeng/toolbar';
import { RatingModule } from 'primeng/rating';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputNumberModule } from 'primeng/inputnumber';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { MessageService } from 'primeng/api';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { CheckboxModule } from 'primeng/checkbox';
import { PanelModule } from 'primeng/panel';
import { PaginatorModule } from 'primeng/paginator';
import { MenuModule } from 'primeng/menu';
import { MenubarModule } from 'primeng/menubar';
import { ProductService } from '../services/productservice';

@NgModule({
  declarations: [],
  imports: [
    TableModule,
    ToastModule,
    RatingModule,
    ConfirmDialogModule,
    ToolbarModule,
    FileUploadModule,
    DialogModule,
    InputNumberModule,
    DropdownModule,
    CheckboxModule,
    CalendarModule,
    PanelModule,
    PaginatorModule,
    MenuModule,
    MenubarModule,
  ],
  exports: [
    TableModule,
    ToastModule,
    RatingModule,
    ConfirmDialogModule,
    ToolbarModule,
    FileUploadModule,
    DialogModule,
    InputNumberModule,
    DropdownModule,
    CheckboxModule,
    CalendarModule,
    PanelModule,
    PaginatorModule,
    MenuModule,
    MenubarModule,
  ],
  providers: [MessageService, ConfirmationService, ProductService],
})
export class SharedModule {}
