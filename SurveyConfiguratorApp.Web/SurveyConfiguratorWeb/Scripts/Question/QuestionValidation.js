
const QUESTION_TEXT_MAX_LENGTH = 1500;
const QUESTION_TEXT_MIN_LENGTH = 10;

const QUESTION_ORDER_MAX_VALUE = 100;
const QUESTION_ORDER_MIN_VALUE = 1;
let isValidOrder = false;
let isValidText = false;




const SLIDER_CAPTION_TEXT_LENGTH_MAX = 500;
const SLIDER_CAPTION_TEXT_LENGTH_MIN = 3;
const SLIDER_MAX_VALUE = 100;
const SLIDER_MIN_VALUE = 0;
let isValidStartValueNumber = false;
let isValidEndValueNumber = false;

let isValidStartCaption = false;
let isValidEndCaption = false;

const STARS_MAX_VALUE = 10;
const STARS_MIN_VALUE = 1;
let isValidStarsNumber = false;

const FACES_MAX_VALUE = 5;
const FACES_MIN_VALUE = 2;
let isValidFacesNumber = false;


function HandleBorderInput(inputs, isSuccess) {
    if (isSuccess) {
        inputs.classList.remove('border-danger');
        inputs.classList.add('border-success');
    }
    else {
        inputs.classList.remove('border-success');
        inputs.classList.add('border-danger');
    }
}

