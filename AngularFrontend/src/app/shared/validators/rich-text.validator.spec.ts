import { RichTextRequired } from '../validators/rich-text.validator';

describe('TextRequired', () => {
  it('should create an instance', () => {
    expect(new RichTextRequired()).toBeTruthy();
  });
});
