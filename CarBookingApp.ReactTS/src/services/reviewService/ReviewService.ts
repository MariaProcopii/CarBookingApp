import { AxiosInstance } from 'axios';
import { CreateReviewParams } from './types';

function ReviewService(instance: AxiosInstance | null) {

    async function createReview(
        createReviewParams: CreateReviewParams,
        setSnackbar: (obj: {open: boolean; message: string; severity: string;}) => void
    ): Promise<void> {
        try {
            await instance?.post("/review/create", createReviewParams);
            setSnackbar({ open: true, message: "Review created successfully!", severity: "success" });
          } catch (error) {
            console.error('Error creating payment:', error);
            setSnackbar({ open: true, message: "Failed to create review!", severity: "error" });
          }
    }

  return {
    createReview
  };
}

export default ReviewService;
