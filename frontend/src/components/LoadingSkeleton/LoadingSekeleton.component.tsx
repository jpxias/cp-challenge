import { Skeleton, Stack } from "@mui/material";

const LoadingSkeleton = () => {
  const renderSkeletons = () => {
    return [...Array(5)].map((_, i) => (
      <Skeleton key={i} variant="rectangular" width="100%" height={50} />
    ));
  };

  return <Stack spacing={2}>{renderSkeletons()}</Stack>;
};

export default LoadingSkeleton;
