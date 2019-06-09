import { Component, OnInit } from '@angular/core';
import { ProductTypeResponse } from '@_generated/models';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProductTypeService } from '@_generated/services';
import { Router } from '@angular/router';
import { ValidationConstants } from '@_models';

@Component({
  selector: 'app-productTypeCreate',
  templateUrl: './productTypeCreate.component.html',
  styleUrls: ['./productTypeCreate.component.scss']
})
export class ProductTypeCreateComponent implements OnInit {
  public productType: ProductTypeResponse = {};
  public submitted: boolean;
  public productTypeForm: FormGroup;

  public imagePath;
  imgURL: any;
  public message: string;

  constructor(
    private productTypeService: ProductTypeService,
    private formBuilder: FormBuilder,
    private router: Router
  ) { }

  ngOnInit() {
    this.productTypeForm = this.formBuilder.group({
      productTypeImage: [''],
      productTypeName: ['', [Validators.required, Validators.minLength(ValidationConstants.MinNameLength), Validators.maxLength(ValidationConstants.MaxNameLength)]],
      ean: ['', Validators.required],
      price: ['', [Validators.required, Validators.min(0.01)]],
      description: ['']
    });
  }


  // convenience getter for easy access to form fields
  get f() { return this.productTypeForm.controls; }

  // getter for ValidationConstants
  get valditationConstants() { return ValidationConstants; }

  preview(files) {
    if (files.length === 0)
      return;

    var mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      this.message = "Only images are supported.";
      return;
    }

    var reader = new FileReader();
    this.imagePath = files;

    reader.readAsDataURL(files[0]);
    reader.onload = (_event) => {
      this.f.productTypeImage.setValue(reader.result);
      this.imgURL = reader.result;
    }
  }


  /** click event methods for **/
  createClicked(): void {
    this.submitted = true;

    if (this.productTypeForm.invalid) {
      return;
    }

    this.productTypeService.CreateProductType({
      imageBase64: this.imgURL.replace(/^data:image\/[a-z]+;base64,/, ""),
      name: this.f.productTypeName.value, ean: this.f.ean.value, price: this.f.price.value, description: this.f.description.value
    }).subscribe(data => {
      alert("ProductType created succesfully");
      this.router.navigate(['/productTypes']);
    }, (error) => {
      alert(error);
    })
  }
}